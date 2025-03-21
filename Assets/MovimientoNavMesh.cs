using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class MovimientoNavMesh : MonoBehaviour
{
    public Transform[] waypoints; // Lista de waypoints originales
    private List<Transform> waypointsDisponibles = new List<Transform>(); // Lista de waypoints sin repetir
    private NavMeshAgent agent;
    public TMP_Text mensajeFinalizacion; // Mensaje en pantalla
    public float tiempoEsperaAntesDeCerrar = 3f; // Tiempo antes de cerrar el juego
    public static List<MovimientoNavMesh> enemigos = new List<MovimientoNavMesh>(); // Lista de todos los enemigos
    public GameObject player; // Referencia al jugador para congelarlo cuando termine el juego
    public TMP_Text mensajeCanvas; // 🔹 Referencia al texto en el Canvas
    public float distanciaHuida = 1f; // Distancia mínima para huir del player
    private float tiempoUltimaHuida = -999f;
    public float tiempoEntreHuidas = 0.5f; // espera al menos 2 segundos para volver a huir
    private int contadorHuidas = 0;







    public float velocidadGiro = 500f; // 🟢 Nueva variable para ajustar la velocidad del giro

    private int targetRotation = 0; // Rotación objetivo (0°, 90°, 180°, 270°)
    private bool isRotating = false; // Indica si el enemigo está girando
    private Transform currentWaypoint; // Waypoint actual al que se dirige

    private bool persiguiendoPlayer = false; // 🔹 Indica si está persiguiendo al player
    private float tiempoInicioPersecucion; // 🔹 Guarda el tiempo cuando empezó la persecución

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.enabled = false;
        Vector3 posicionDeseada = transform.position;
        posicionDeseada.y = 1;
        transform.position = posicionDeseada;
        agent.enabled = true;

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        waypointsDisponibles = new List<Transform>(waypoints);

        if (waypointsDisponibles.Count > 0)
        {
            MoverAlSiguientePunto();
        }
        else
        {
            StartCoroutine(PartidaFinalizada());
        }
    }

    void Update()
    {
        {
            if (isRotating)
            {
                RotarHaciaObjetivo();
                return;
            }

            float distancia = Vector3.Distance(transform.position, player.transform.position);

            if(distancia < distanciaHuida && Time.time - tiempoUltimaHuida > tiempoEntreHuidas)
            {
                // 🔥 Huir del jugador
                Transform waypointMasLejano = null;
                float mayorDistancia = 0f;

                foreach (Transform wp in waypointsDisponibles)
                {
                    float d = Vector3.Distance(wp.position, player.transform.position);
                    if (d > mayorDistancia)
                    {
                        mayorDistancia = d;
                        waypointMasLejano = wp;
                    }
                }

                if (waypointMasLejano != null)
                {
                    currentWaypoint = waypointMasLejano;

                    Vector3 direccion = (currentWaypoint.position - transform.position).normalized;
                    int nuevaRotacion = targetRotation;

                    if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
                        nuevaRotacion = direccion.x > 0 ? 90 : 270;
                    else
                        nuevaRotacion = direccion.z > 0 ? 0 : 180;

                    if (nuevaRotacion != targetRotation)
                    {
                        targetRotation = nuevaRotacion;
                        isRotating = true;
                    }
                    else
                    {
                        agent.SetDestination(currentWaypoint.position);
                    }

                    tiempoUltimaHuida = Time.time; // <-- para controlar el tiempo entre huidas
                    tiempoInicioPersecucion = Time.time;                    
                    persiguiendoPlayer = true;
                    contadorHuidas++;
                    Debug.Log("🏃‍♂️ Huyendo del jugador - Veces que ha huido: " + contadorHuidas);


                   
                }

                return;
            }

            if (persiguiendoPlayer)
            {
                // Si llegó al punto de huida, vuelve a patrullar
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    Debug.Log("✅ Enemigo ha llegado al punto lejano. Fin de la huida.");
                    persiguiendoPlayer = false;
                    MoverAlSiguientePunto();
                    return;
                }

                Vector3 direccion = (player.transform.position - transform.position).normalized;
                Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, velocidadGiro * Time.deltaTime);

                if (Time.time - tiempoInicioPersecucion >= 10f)
                {
                    persiguiendoPlayer = false;
                    Debug.Log("⏳ Se acabó la persecución, volviendo a waypoints.");
                    MoverAlSiguientePunto();
                }
                else
                {
                    // Mantener la posición, no perseguir al player mientras huye
                    // Solo sigue y completa el camino al waypoint ya asignado

                }

                return;
            }

            if (waypointsDisponibles.Count == 0)
            {
                StartCoroutine(PartidaFinalizada());
                return;
            }

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                MoverAlSiguientePunto();
            }
        }




        void RotarHaciaObjetivo()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, targetRotation, 0), velocidadGiro * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, targetRotation, 0)) < 1f)
            {
                isRotating = false;
                agent.SetDestination(currentWaypoint.position);
            }
        }

    }

    void LateUpdate()
    {
        Vector3 posicionCorrigida = transform.position;
        posicionCorrigida.y = 1;
        transform.position = posicionCorrigida;
    }

    public void MoverAlSiguientePunto()
    {

        if (waypointsDisponibles.Count == 0)
        {
            StartCoroutine(PartidaFinalizada());
            return;
        }

        currentWaypoint = waypointsDisponibles[Random.Range(0, waypointsDisponibles.Count)];
        

        Vector3 direccion = (currentWaypoint.position - transform.position).normalized;
        int nuevaRotacion = targetRotation;

        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
        {
            nuevaRotacion = direccion.x > 0 ? 90 : 270;
        }
        else
        {
            nuevaRotacion = direccion.z > 0 ? 0 : 180;
        }

        if (nuevaRotacion != targetRotation)
        {
            targetRotation = nuevaRotacion;
            isRotating = true;
        }
        else
        {
            agent.SetDestination(currentWaypoint.position);
        }
    }

    void RotarHaciaObjetivo()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, targetRotation, 0), velocidadGiro * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, targetRotation, 0)) < 1f)
        {
            isRotating = false;
            agent.SetDestination(currentWaypoint.position);
        }
    }
    void OnDrawGizmosSelected()
    {
        

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaHuida);
    }
    void HuirDelJugador()
    {
        Transform waypointMasLejano = null;
        float mayorDistancia = 0f;

        foreach (Transform wp in waypointsDisponibles)
        {
            float d = Vector3.Distance(wp.position, player.transform.position);
            if (d > mayorDistancia)
            {
                mayorDistancia = d;
                waypointMasLejano = wp;
            }
        }

        if (waypointMasLejano != null)
        {
            currentWaypoint = waypointMasLejano;
            Vector3 direccion = (currentWaypoint.position - transform.position).normalized;
            int nuevaRotacion = targetRotation;

            if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
                nuevaRotacion = direccion.x > 0 ? 90 : 270;
            else
                nuevaRotacion = direccion.z > 0 ? 0 : 180;

            if (nuevaRotacion != targetRotation)
            {
                targetRotation = nuevaRotacion;
                isRotating = true;
            }
            else
            {
                agent.SetDestination(currentWaypoint.position);
            }

            tiempoUltimaHuida = Time.time;      // 🔥 importante
            contadorHuidas++;
            Debug.Log("🏃‍♂️ Huyendo del jugador (" + contadorHuidas + ")");
        }
    }



    IEnumerator PartidaFinalizada()
    {
        Debug.Log("✅ Todos los waypoints han sido visitados. Se detiene el juego.");

        if (mensajeFinalizacion != null)
        {
            mensajeFinalizacion.text = "¡Partida terminada! No quedan más objetivos.";
        }

        foreach (MovimientoNavMesh enemigo in enemigos)
        {
            if (enemigo.agent != null)
            {
                enemigo.agent.isStopped = true;
            }
        }

        if (player != null)
        {
            NavMeshAgent playerAgent = player.GetComponent<NavMeshAgent>();
            if (playerAgent != null)
            {
                playerAgent.isStopped = true;
            }

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.Sleep();
            }
        }

        yield return new WaitForSeconds(tiempoEsperaAntesDeCerrar);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("enemy"))
        {
            if (other.CompareTag("poder1"))
            {
                agent.speed = 10;
                Debug.Log("🚀 ¡Velocidad aumentada a 10!");
                Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("poder2") && gameObject.CompareTag("Player"))
        {
            foreach (MovimientoNavMesh enemigo in enemigos)
            {
                enemigo.agent.speed = 2;
            }
            Debug.Log("🐢 ¡Velocidad de los enemigos reducida a 2!");
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Player") && gameObject.CompareTag("enemy"))
        {
            Debug.Log("Enhorabuena crack, los fantasmas han ganado hoy!");

            // Mostrar mensaje en pantalla antes de cerrar el juego
            if (mensajeCanvas != null)
            {
                mensajeCanvas.text = "¡Enhorabuena crack, los fantasmas han ganado!";
                mensajeCanvas.gameObject.SetActive(true); // Asegurar que el texto se muestre
                mensajeCanvas.ForceMeshUpdate();
            }

            Time.timeScale = 0;

            // Esperar 3 segundos antes de cerrar el juego
            StartCoroutine(ReanudarTiempoYCerrar());
        


        }

       

        if (persiguiendoPlayer && other.CompareTag("Player"))
        {
            Debug.Log("😂 JAJAJAJA HOY GANA PAC-MAN");

            // Mostrar el mensaje en pantalla si existe el texto en el Canvas
            if (mensajeCanvas != null)
            {
                mensajeCanvas.text = "JAJAJAJA HOY GANA PAC-MAN";
                mensajeCanvas.gameObject.SetActive(true); // 🔥 Asegurarse de que el texto se muestre
            }

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        // 🔄 🔥 Si el enemigo toca cualquier otro objeto y se queda bloqueado, restaurar su movimiento
        if (!persiguiendoPlayer && agent.isStopped)
        {
            Debug.Log("⚠️ Enemigo bloqueado tras colisión, restaurando movimiento.");
            agent.isStopped = false;
            MoverAlSiguientePunto();
        }

        IEnumerator ReanudarTiempoYCerrar()
        {
            yield return new WaitForSeconds(3f); // ⏳ Esperar 3 segundos antes de cerrar

            Time.timeScale = 1;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }

    }



}
