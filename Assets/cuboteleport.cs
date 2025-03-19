using UnityEngine;
using System.Collections;

public class Teletransportador : MonoBehaviour
{
    public Transform destino;
    public float cooldownTime = 5f; // ⏳ Tiempo de espera antes de volver a teletransportar
    private bool enCooldown = false;

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

        // 🔴 Desactivar el collider del objeto para evitar doble teletransporte
        Collider objCollider = objeto.GetComponent<Collider>();
        if (objCollider != null) objCollider.enabled = false;

        // Desactivar el teletransportador temporalmente
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Verificar si el objeto tiene NavMeshAgent y detenerlo
        UnityEngine.AI.NavMeshAgent agente = objeto.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agente != null)
        {
            agente.isStopped = true;  // ✋ Detiene el NavMeshAgent antes del teletransporte
            agente.enabled = false;   // 🔧 Desactiva el NavMeshAgent
        }

        // Verificar si tiene un Rigidbody y desactivar temporalmente la física
        Rigidbody rb = objeto.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;   // 🛑 Desactiva la física para evitar movimientos raros
            rb.linearVelocity = Vector3.zero; // ✨ Limpia cualquier movimiento residual
        }

        // Si el objeto es un jugador con CharacterController, desactivarlo
        CharacterController cc = objeto.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }

        // Teletransportar al destino
        objeto.transform.position = destino.position + Vector3.up * 1.5f;

        yield return new WaitForSeconds(0.5f); // Esperar para actualizar la posición correctamente

        // Reactivar NavMeshAgent si lo tenía
        if (agente != null)
        {
            agente.enabled = true;
            agente.isStopped = false;
            agente.SetDestination(destino.position); // 💡 Recalcular camino
        }

        // Reactivar CharacterController si lo tenía
        if (cc != null)
        {
            yield return new WaitForSeconds(0.2f); // Esperar un poco más
            cc.enabled = true;
        }

        // Reactivar Rigidbody si lo tenía
        if (rb != null)
        {
            yield return new WaitForSeconds(0.2f);
            rb.isKinematic = false; // 🔄 Reactivar la física
        }

        // Reactivar el collider del objeto después de 1 segundo para evitar bucles
        yield return new WaitForSeconds(1f);
        if (objCollider != null) objCollider.enabled = true;

        // ⏳ Esperar antes de reactivar el teletransportador (5 segundos)
        yield return new WaitForSeconds(cooldownTime - 1f);

        if (col != null) col.enabled = true; // ✅ Reactivar el teletransportador

        enCooldown = false;
    }
}



