using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
    public Teleport otroTeletransportador; // Referencia al otro teletransportador
    private bool enCooldown = false; // Evitar bucles de teletransporte

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !enCooldown) // Si es el Player y no está en cooldown
        {
            StartCoroutine(Teletransportar(other));
        }
    }

    private IEnumerator Teletransportar(Collider objeto)
    {
        enCooldown = true; // Activa cooldown para evitar múltiples activaciones
        otroTeletransportador.enCooldown = true; // También activa cooldown en el otro teletransportador

        // Mantiene la altura del objeto
        Vector3 nuevaPosicion = otroTeletransportador.transform.position;
        nuevaPosicion.y = objeto.transform.position.y;

        yield return new WaitForSeconds(0.1f); // Pequeña espera antes de moverlo
        objeto.transform.position = nuevaPosicion;

        yield return new WaitForSeconds(0.5f); // Esperar para evitar que el otro teletransportador se active de inmediato

        enCooldown = false; // Se permite volver a teletransportar
        otroTeletransportador.enCooldown = false; // También se reactiva el otro teletransportador
    }
}
