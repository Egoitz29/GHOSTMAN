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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 5; // Velocidad inicial

        // Agregar este enemigo a la lista global
        if (gameObject.CompareTag("enemy"))
        {
            enemigos.Add(this);
        }

        // Copiar los waypoints originales a la lista de disponibles
        waypointsDisponibles = new List<Transform>(waypoints);

        if (waypointsDisponibles.Count > 0)
        {
            MoverAlSiguientePunto();
        }
        else
        {
            StartCoroutine(PartidaFinalizada()); // Si no hay waypoints desde el inicio, cerrar juego
        }
    }

    void Update()
    {
        // 🔹 Si ya no quedan waypoints en la lista, iniciar el cierre del juego
        if (waypointsDisponibles.Count == 0)
        {
            StartCoroutine(PartidaFinalizada());
            return;
        }

        // Si el agente llegó al destino y aún quedan waypoints, elegir uno nuevo
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoverAlSiguientePunto();
        }
    }

    void MoverAlSiguientePunto()
    {
        // 🔹 Eliminar todos los waypoints que sean null antes de elegir uno
        waypointsDisponibles.RemoveAll(w => w == null);

        if (waypointsDisponibles.Count == 0)
        {
            StartCoroutine(PartidaFinalizada());
            return;
        }

        int indiceAleatorio = Random.Range(0, waypointsDisponibles.Count);
        Transform waypointSeleccionado = waypointsDisponibles[indiceAleatorio];

        waypointsDisponibles.RemoveAt(indiceAleatorio);
        agent.destination = waypointSeleccionado.position;
    }

    IEnumerator PartidaFinalizada()
    {
        Debug.Log("✅ Todos los waypoints han sido visitados o eliminados. Cerrando juego...");

        if (mensajeFinalizacion != null)
        {
            mensajeFinalizacion.text = "Has perdido, pequeño enano"; // 🔹 Mensaje antes de cerrar el juego
        }

        agent.isStopped = true; // Detener el movimiento

        yield return new WaitForSeconds(tiempoEsperaAntesDeCerrar); // ⏳ Esperar antes de cerrar el juego

        // 🔹 Detener el juego en el editor de Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 🔹 Cerrar el juego si está compilado (EXE, APK, etc.)
        Application.Quit();
#endif
    }

    // 🔹 Detectar colisión con objetos de poder y modificar velocidad
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("enemy"))
        {
            // Poder 1: Aumenta la velocidad del enemigo
            if (other.CompareTag("poder1"))
            {
                agent.speed = 10;
                Debug.Log("🚀 ¡Velocidad aumentada a 10!");
                Destroy(other.gameObject); // 🔥 Eliminar la esfera "poder1" al contacto
            }
        }

        // 🔥 Si el "player" toca un "poder2", baja la velocidad de TODOS los enemigos
        if (other.CompareTag("poder2") && gameObject.CompareTag("Player"))
        {
            foreach (MovimientoNavMesh enemigo in enemigos)
            {
                enemigo.agent.speed = 2; // Reducir la velocidad a 2
            }
            Debug.Log("🐢 ¡Velocidad de los enemigos reducida a 2!");
            Destroy(other.gameObject); // 🔥 Eliminar la esfera "poder2" al contacto
        }

        // 🔥 Si el "player" toca un "enemy", mostrar mensaje en consola y cerrar el juego
        if (other.CompareTag("Player") && gameObject.CompareTag("enemy"))
        {
            Debug.Log(" Enhorabuena crack, los fantasmitas han ganado hoy!");

            // 🔹 Detener el juego en el editor de Unity
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // 🔹 Cerrar el juego si está compilado (EXE, APK, etc.)
            Application.Quit();
#endif
        }
    }
}

