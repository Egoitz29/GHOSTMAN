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

    public float velocidadGiro = 500f; // 🟢 Nueva variable para ajustar la velocidad del giro

    private int targetRotation = 0; // Rotación objetivo (0°, 90°, 180°, 270°)
    private bool isRotating = false; // Indica si el enemigo está girando
    private Transform currentWaypoint; // Waypoint actual al que se dirige

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 5;

        // 🟢 Desactivar temporalmente el NavMeshAgent para evitar ajustes de altura
        agent.enabled = false;

        // 🟢 Ajustar la posición deseada
        Vector3 posicionDeseada = transform.position;
        posicionDeseada.y = 1; // 🔥 Fijar la altura
        transform.position = posicionDeseada;

        // 🟢 Reactivar el NavMeshAgent después de fijar la posición
        agent.enabled = true;

        // Evitar que el NavMeshAgent rote automáticamente
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Copiar los waypoints originales a la lista de disponibles
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
        // 🔴 Si está girando, solo gira y no avanza
        if (isRotating)
        {
            RotarHaciaObjetivo();
            return;
        }

        // Si ya no quedan waypoints, finalizar la partida
        if (waypointsDisponibles.Count == 0)
        {
            StartCoroutine(PartidaFinalizada());
            return;
        }

        // Si el enemigo llega al destino, buscar otro
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoverAlSiguientePunto();
        }
    }

    void LateUpdate()
    {
        Vector3 posicionCorrigida = transform.position;
        posicionCorrigida.y = 1; // 🔥 Asegurar que la altura sea 1
        transform.position = posicionCorrigida;
    }


    // 🔹 Buscar el siguiente waypoint y ajustar la dirección
    void MoverAlSiguientePunto()
    {
        if (waypointsDisponibles.Count == 0)
        {
            StartCoroutine(PartidaFinalizada());
            return;
        }

        currentWaypoint = waypointsDisponibles[Random.Range(0, waypointsDisponibles.Count)];
        waypointsDisponibles.Remove(currentWaypoint);

        // Calcular dirección hacia el nuevo waypoint
        Vector3 direccion = (currentWaypoint.position - transform.position).normalized;

        // Definir rotación exacta en pasos de 90°
        int nuevaRotacion = targetRotation;

        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
        {
            nuevaRotacion = direccion.x > 0 ? 90 : 270;
        }
        else
        {
            nuevaRotacion = direccion.z > 0 ? 0 : 180;
        }

        // Si hay un cambio de dirección, girar primero
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

    // 🔹 Girar más rápido hacia la rotación objetivo
    void RotarHaciaObjetivo()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, targetRotation, 0), velocidadGiro * Time.deltaTime);

        // Si ya giró completamente, permitir moverse
        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, targetRotation, 0)) < 1f)
        {
            isRotating = false;
            agent.SetDestination(currentWaypoint.position); // 🟢 Ahora sí se mueve después de girar
        }
    }

    IEnumerator PartidaFinalizada()
    {
        Debug.Log("✅ Todos los waypoints han sido visitados. Se detiene el juego.");

        // 🛑 Mostrar mensaje en pantalla
        if (mensajeFinalizacion != null)
        {
            mensajeFinalizacion.text = "¡Partida terminada! No quedan más objetivos.";
        }

        // 🛑 Detener a todos los enemigos
        foreach (MovimientoNavMesh enemigo in enemigos)
        {
            if (enemigo.agent != null)
            {
                enemigo.agent.isStopped = true;
            }
        }

        // 🛑 Detener al jugador si tiene un `NavMeshAgent`
        if (player != null)
        {
            NavMeshAgent playerAgent = player.GetComponent<NavMeshAgent>();
            if (playerAgent != null)
            {
                playerAgent.isStopped = true;
            }

            // 🛑 Si el jugador usa Rigidbody, congelar su movimiento
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.Sleep(); // ✅ Método recomendado en Unity 2023 en lugar de rb.velocity = Vector3.zero;
            }
        }

        // ⏳ Esperar unos segundos antes de cerrar el juego
        yield return new WaitForSeconds(tiempoEsperaAntesDeCerrar);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }


    // 🔹 Detectar colisión con poderes y modificar velocidad
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("enemy"))
        {
            // 🔥 Si el enemigo toca "poder1", aumenta su velocidad
            if (other.CompareTag("poder1"))
            {
                agent.speed = 10;
                Debug.Log("🚀 ¡Velocidad aumentada a 10!");
                Destroy(other.gameObject);
            }
        }

        // 🔥 Si el "player" toca "poder2", baja la velocidad de TODOS los enemigos
        if (other.CompareTag("poder2") && gameObject.CompareTag("Player"))
        {
            foreach (MovimientoNavMesh enemigo in enemigos)
            {
                enemigo.agent.speed = 2;
            }
            Debug.Log("🐢 ¡Velocidad de los enemigos reducida a 2!");
            Destroy(other.gameObject);
        }

        // 🔥 Si el "player" toca un "enemy", mostrar mensaje y cerrar el juego
        if (other.CompareTag("Player") && gameObject.CompareTag("enemy"))
        {
            Debug.Log("Enhorabuena crack, los fantasmitas han ganado hoy!");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
