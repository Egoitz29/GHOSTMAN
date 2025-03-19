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

            if (persiguiendoPlayer)
            {
                // 🔄 Rotar gradualmente hacia el Player
                Vector3 direccion = (player.transform.position - transform.position).normalized;
                Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, velocidadGiro * Time.deltaTime);

                // 🔥 Si pasan 10 segundos sin alcanzar al player, volver a la ruta normal
                if (Time.time - tiempoInicioPersecucion >= 10f)
                {
                    persiguiendoPlayer = false;
                    Debug.Log("⏳ Se agotó el tiempo de persecución. Volviendo a waypoints.");
                    MoverAlSiguientePunto();
                }
                else
                {
                    agent.SetDestination(player.transform.position);
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

        void MoverAlSiguientePunto()
        {
            if (waypointsDisponibles.Count == 0)
            {
                StartCoroutine(PartidaFinalizada());
                return;
            }

            currentWaypoint = waypointsDisponibles[Random.Range(0, waypointsDisponibles.Count)];
            waypointsDisponibles.Remove(currentWaypoint);

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

            // 🔥 Si está girando, respetar la rotación
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

    }

    void LateUpdate()
    {
        Vector3 posicionCorrigida = transform.position;
        posicionCorrigida.y = 1;
        transform.position = posicionCorrigida;
    }

    void MoverAlSiguientePunto()
    {
        if (waypointsDisponibles.Count == 0)
        {
            StartCoroutine(PartidaFinalizada());
            return;
        }

        currentWaypoint = waypointsDisponibles[Random.Range(0, waypointsDisponibles.Count)];
        waypointsDisponibles.Remove(currentWaypoint);

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
            Debug.Log("Enhorabuena crack, los fantasmitas han ganado hoy!");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        // 🔥 🔹 Si el enemigo toca "matar", prioriza al player y destruye el objeto
        if (other.CompareTag("matar"))
        {
            Debug.Log("🔴 El enemigo ha tocado 'matar'. Ahora prioriza al player.");
            persiguiendoPlayer = true;
            tiempoInicioPersecucion = Time.time;
            agent.SetDestination(player.transform.position);

            // 🛑 Asegurar que sigue girando y moviéndose después de tocar "matar"
            isRotating = false; // 🔹 Evitar que se quede en un estado de rotación bloqueado
            agent.isStopped = false; // 🔥 Reactivar movimiento

            // 🔄 Forzar el movimiento si se detiene
            if (!agent.hasPath)
            {
                MoverAlSiguientePunto();
            }

            Destroy(other.gameObject); // 🔥 Elimina el objeto con el tag "matar"
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
    }



}
