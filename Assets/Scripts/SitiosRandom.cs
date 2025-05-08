using UnityEngine;
using UnityEngine.AI;

public class SitiosRandom : MonoBehaviour

{
    [SerializeField] private Transform[] puntosSpawn;

    void Start()
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
        }
        else
        {
            Debug.LogError("❌ El punto de spawn no está sobre el NavMesh: " + punto.name);
        }
    }
}



