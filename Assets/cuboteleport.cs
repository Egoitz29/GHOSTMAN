using UnityEngine;
using System.Collections;

public class Teletransportador : MonoBehaviour
{
    public Transform destino;
    private static bool enCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Objeto detectado: {other.gameObject.name}, Tag: {other.tag}"); // 👀 Verifica qué detecta

        if ((other.CompareTag("Player") || other.CompareTag("enemy")) && !enCooldown)
        {
            Debug.Log($"Teletransportando a: {other.gameObject.name}"); // 📌 Confirmación de teletransporte
            StartCoroutine(Teletransportar(other.gameObject));
        }
    }

    private IEnumerator Teletransportar(GameObject objeto)
    {
        enCooldown = true;

        // Verificar si el objeto tiene NavMeshAgent
        UnityEngine.AI.NavMeshAgent agente = objeto.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agente != null)
        {
            agente.isStopped = true;  // ✋ Detiene el NavMeshAgent antes del teletransporte
            agente.enabled = false;    // 🔧 Desactiva el NavMeshAgent
        }

        // Teletransportar al destino
        objeto.transform.position = destino.position + Vector3.up * 1.5f;

        yield return new WaitForSeconds(0.2f); // Espera para asegurar que se actualiza la posición

        if (agente != null)
        {
            agente.enabled = true;   // 🔄 Reactivar el NavMeshAgent
            agente.isStopped = false; // ✅ Reanudar movimiento
            agente.SetDestination(destino.position); // 💡 Forzar nueva ruta al destino
        }

        enCooldown = false;
    }
}











