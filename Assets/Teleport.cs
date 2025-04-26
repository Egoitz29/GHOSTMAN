using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
    public Teleport otroTeletransportador; // Referencia al otro teletransportador
    private bool enCooldown = false; // Evitar bucles de teletransporte

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entró en el trigger con: " + other.name);

        if ((other.CompareTag("Player") || other.CompareTag("enemy")) && !enCooldown)
        {
            Debug.Log("Comienza teletransporte...");
            StartCoroutine(Teletransportar(other));
        }
    }


    private IEnumerator Teletransportar(Collider objeto)
    {
        Debug.Log("Teletransportando a: " + objeto.name);

        enCooldown = true;
        otroTeletransportador.enCooldown = true;

        // Posición de destino
        Vector3 nuevaPosicion = otroTeletransportador.transform.position;
        nuevaPosicion.y = objeto.transform.position.y;

        yield return new WaitForSeconds(0.1f);

        // Revisa si tiene NavMeshAgent
        var nav = objeto.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (nav != null)
        {
            Debug.Log("Usando NavMeshAgent.Warp");
            nav.Warp(nuevaPosicion);
        }
        else
        {
            // Desactivar CharacterController si existe
            var controller = objeto.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                objeto.transform.position = nuevaPosicion;
                controller.enabled = true;
            }
            else
            {
                objeto.transform.position = nuevaPosicion;
            }
        }

        yield return new WaitForSeconds(0.5f);

        enCooldown = false;
        otroTeletransportador.enCooldown = false;
    }

}
