using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PersecucionEnemigo : MonoBehaviour
{
    private NavMeshAgent agent;
    private MovimientoNavMesh movimientoNavMesh;
    public bool persiguiendo { get; private set; } = false;
    private float tiempoInicioPersecucion;
    public float duracionPersecucion = 10f;
    private bool enCooldown = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        movimientoNavMesh = GetComponent<MovimientoNavMesh>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("matar") && !enCooldown)
        {
            Debug.Log("🔴 El enemigo ha tocado 'matar'. Persigue al jugador.");
            StartCoroutine(IniciarPersecucion());
            Destroy(other.gameObject);
        }
    }

    private IEnumerator IniciarPersecucion()
    {
        persiguiendo = true;
        tiempoInicioPersecucion = Time.time;
        movimientoNavMesh.enabled = false; // 🔥 Desactiva su movimiento normal

        while (Time.time - tiempoInicioPersecucion < duracionPersecucion)
        {
            if (agent != null && movimientoNavMesh.player != null)
            {
                agent.SetDestination(movimientoNavMesh.player.transform.position);
            }
            yield return null;
        }

        Debug.Log("⏳ Se acabó el tiempo de persecución. Volviendo a patrullar.");

        // ✅ DETENER AL ENEMIGO PARA QUE NO SIGA PERSIGUIENDO
        agent.ResetPath(); // ❗ Esto lo obliga a detenerse y dejar de seguir al jugador.
        yield return new WaitForSeconds(0.5f); // Pequeña pausa para evitar errores

        movimientoNavMesh.enabled = true; // 🔥 Reactivar patrulla
        persiguiendo = false;
        enCooldown = true;

        // ✅ Asegurar que se mueva a un waypoint nuevo
        movimientoNavMesh.MoverAlSiguientePunto();

        yield return new WaitForSeconds(3f);
        enCooldown = false;
    }
}
