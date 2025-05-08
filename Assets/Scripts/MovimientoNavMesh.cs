using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;


public class MovimientoNavMesh : MonoBehaviour
{
    public Transform[] waypoints;
    private NavMeshAgent agent;
    public TMP_Text mensajeFinalizacion;
    public float tiempoEsperaAntesDeCerrar = 3f;
    public static List<MovimientoNavMesh> enemigos = new List<MovimientoNavMesh>();
    public GameObject player;
    public TMP_Text mensajeCanvas;
    public float distanciaHuida = 0.1f;
    private float tiempoUltimaHuida = -999f;
    public float tiempoEntreHuidas = 2f;
    public float velocidadGiro = 500f;

    private static int reintentos = 0;
    private int maxIntentos = 2; // reiniciar 2 veces → 3 partidas en total
    private static bool escenaCambiada = false; // Para evitar que múltiples enemigos cambien de escena o cierren el juego

    [SerializeField] private TextMeshProUGUI vidasCanvas;
    [SerializeField] private GameObject canvasFinal;  // 👈 referencia al panel final dentro de tu Canvas
    [SerializeField] private float velocidadExtraHuida = 5f;
    [SerializeField] private float duracionVelocidadExtra = 3f;

    private float velocidadOriginal;
    private Coroutine restaurarVelocidadCoroutine;

    private int targetRotation = 0;
    private bool isRotating = false;
    private Transform currentWaypoint;

    void Start()
    {


        agent = GetComponent<NavMeshAgent>();

        // ✅ Deja que el NavMeshAgent controle rotación y altura
        agent.updateRotation = true;
        agent.updateUpAxis = true;

        // 🔧 Asegúrate de que está activado
        agent.enabled = true;

        if (waypoints.Length > 0)
            MoverAlSiguientePunto();
        else
            Debug.LogError("❌ No se han asignado waypoints al enemigo.");
      
        
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        velocidadOriginal = agent.speed;

        MostrarVidasRestantes();


    }

    [SerializeField] private Transform[] puntosSpawn;

    private void Awake()
    {
        if (puntosSpawn.Length == 0)
        {
            Debug.LogError("⚠️ No hay puntos de spawn asignados.");
            return;
        }

        int index = Random.Range(0, puntosSpawn.Length);
        Transform punto = puntosSpawn[index];

        // Asegúrate de que el punto esté sobre el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(punto.position, out hit, 2f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            transform.rotation = punto.rotation;
            Debug.Log("📍 Spawn aleatorio aplicado en Awake: " + hit.position);
        }
        else
        {
            Debug.LogError("❌ El punto de spawn no está sobre el NavMesh: " + punto.name);
        }
    }

    void Update()
    {

        // 🛡 Protección total: si se perdió la referencia, intenta recuperarla
        if (player == null)
        {
            if (player == null)
            {
                Debug.LogWarning("🚫 El objeto player sigue siendo null");
                return;
            }

            GameObject encontrado = GameObject.Find("ghost"); // 👈 busca por nombre
            if (encontrado != null)
            {
                player = encontrado;
                Debug.Log("♻️ Se reasignó el player al objeto llamado 'ghost'.");
            }
            else
            {
                // ⚠️ Si no lo encuentra, salimos sin hacer nada
                Debug.LogWarning("❌ No se encontró el objeto 'ghost' en escena.");
                return;
            }
        }
       

  


        if (isRotating)
        {
            RotarHaciaObjetivo();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoverAlSiguientePunto();
        }

        float distancia = Vector3.Distance(transform.position, player.transform.position);




        if (distancia < distanciaHuida && Time.time - tiempoUltimaHuida > tiempoEntreHuidas)
        {
            Debug.Log("⚠️ Modo huida activado");
            Transform waypointSeguro = null;
            float mejorPuntaje = -Mathf.Infinity;
            float distanciaSeguridad = 5f;

            Vector3 direccionHuida = (transform.position - player.transform.position).normalized;

            foreach (Transform wp in waypoints)
            {
                if (wp == null) continue;

                float distanciaAlJugador = Vector3.Distance(wp.position, player.transform.position);
                if (distanciaAlJugador < distanciaSeguridad) continue; // sigue si el punto está muy cerca del jugador

                Vector3 direccionAlWP = (wp.position - transform.position).normalized;
                float alineacion = Vector3.Dot(direccionHuida, direccionAlWP); // +1 si está justo en dirección opuesta al jugador

                float distanciaAlEnemigo = Vector3.Distance(wp.position, transform.position);
                float puntaje = alineacion * distanciaAlEnemigo; // huimos lejos y en buena dirección

                if (puntaje > mejorPuntaje)
                {
                    mejorPuntaje = puntaje;
                    waypointSeguro = wp;
                }
            }


            if (waypointSeguro != null)
            {
                currentWaypoint = waypointSeguro;
                Debug.Log("➡️ Huyendo hacia: " + waypointSeguro.name);

                transform.LookAt(currentWaypoint);
                agent.SetDestination(currentWaypoint.position);
                tiempoUltimaHuida = Time.time;

                agent.speed = velocidadOriginal + velocidadExtraHuida;

                if (restaurarVelocidadCoroutine != null)
                    StopCoroutine(restaurarVelocidadCoroutine);

                restaurarVelocidadCoroutine = StartCoroutine(RestaurarVelocidadDespuesDe(duracionVelocidadExtra));
            }
        }


        ActualizarRotacionVisual();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.1f); // control visual
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanciaHuida);
    }



    void RotarHaciaObjetivo()
    {
        Quaternion rotObjetivo = Quaternion.Euler(0, targetRotation, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotObjetivo, velocidadGiro * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, rotObjetivo) < 1f)
        {
            transform.rotation = rotObjetivo;
            isRotating = false;
            agent.SetDestination(currentWaypoint.position);
        }
    }

    public void MoverAlSiguientePunto()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("⚠️ No hay waypoints asignados.");
            return;
        }

        // ✅ Filtrar los waypoints válidos
        List<Transform> waypointsValidos = new List<Transform>();
        foreach (Transform wp in waypoints)
        {
            if (wp != null) waypointsValidos.Add(wp);
        }

        if (waypointsValidos.Count == 0)
        {
            Debug.LogError("❌ Todos los waypoints han sido destruidos o son nulos.");
            return;
        }

        Transform nuevoDestino;
        do
        {
            nuevoDestino = waypointsValidos[Random.Range(0, waypointsValidos.Count)];
        } while (nuevoDestino == currentWaypoint && waypointsValidos.Count > 1);

        currentWaypoint = nuevoDestino;

        Vector3 direccion = currentWaypoint.position - transform.position;

        Vector3 puntoIntermedio = transform.position;
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
            puntoIntermedio += new Vector3(direccion.x, 0, 0);
        else
            puntoIntermedio += new Vector3(0, 0, direccion.z);

        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
            targetRotation = direccion.x > 0 ? 90 : 270;
        else
            targetRotation = direccion.z > 0 ? 0 : 180;

        isRotating = true;

        agent.SetDestination(puntoIntermedio);
    }

    void ActualizarRotacionVisual()
    {
        Vector3 direccionMovimiento = agent.velocity.normalized;

        if (direccionMovimiento.magnitude > 0.1f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadGiro * Time.deltaTime);
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("enemy"))
        {
            // ⏸️ Congelar el tiempo
            Time.timeScale = 0f;

            if (reintentos < maxIntentos)
            {
                reintentos++;
                MostrarVidasRestantes();
                StartCoroutine(ReiniciarPartidaConDelay());
            }
            else
            {
                if (mensajeCanvas != null)
                {
                    mensajeCanvas.text = "¡Enhorabuena crack, los fantasmas han ganado!";
                    mensajeCanvas.gameObject.SetActive(true);
                    mensajeCanvas.ForceMeshUpdate();
                }

                reintentos = 0;

                // ✅ Mostrar el panel final en lugar de cambiar de escena automáticamente
                if (canvasFinal != null)
                {
                    canvasFinal.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("⚠️ No se ha asignado el Panel Final en el inspector.");
                }
            }
        }
    }


    IEnumerator CargarNuevaEscena()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        if (!escenaCambiada)
        {
            escenaCambiada = true;
            SceneManager.LoadScene("Menu");
        }
    }


    private void MostrarVidasRestantes()
    {
        int vidasRestantes = (maxIntentos - reintentos + 1);


        if (vidasCanvas != null)
        {
            if (vidasRestantes > 1)
                vidasCanvas.text = "VIDAS PAC-MAN: " + vidasRestantes;
            else if (vidasRestantes == 1)
                vidasCanvas.text = "Te queda 1 partida";
            else
                vidasCanvas.text = ""; // nada, ya ganó o terminó
        }
    }


    private IEnumerator RestaurarVelocidadDespuesDe(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        agent.speed = velocidadOriginal;
    }
    private IEnumerator ReiniciarPartidaConDelay()
    {
        yield return new WaitForSecondsRealtime(2f); // 👈 se usa "Realtime" porque Time.timeScale está en 0
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator ReanudarTiempoYCerrar()

    {
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
    }
}
