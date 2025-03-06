using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovimientoNavMesh : MonoBehaviour
{
    public Transform[] waypoints; // Puntos a seguir en el mapa
    private int indiceActual = 0; // Índice del waypoint actual
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0)
        {
            MoverAlSiguientePunto();
        }
    }

    void Update()
    {
        // Si el agente llegó al destino, pasar al siguiente waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoverAlSiguientePunto();
        }
    }

    void MoverAlSiguientePunto()
    {
        if (waypoints.Length == 0) return; // Evita errores si no hay waypoints

        agent.destination = waypoints[indiceActual].position; // Mueve al siguiente punto
        indiceActual = (indiceActual + 1) % waypoints.Length; // Ciclo infinito de waypoints
    }
}




