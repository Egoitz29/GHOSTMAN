using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeletransportadorDoble : MonoBehaviour
{
    public TeletransportadorDoble otroTeletransportador; // Referencia al otro teletransportador
    private HashSet<GameObject> objetosEnTeletransporte = new HashSet<GameObject>(); // Evitar bucle

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Enemy")) && !objetosEnTeletransporte.Contains(other.gameObject))
        {
            StartCoroutine(Teletransportar(other.gameObject));
        }
    }

    private IEnumerator Teletransportar(GameObject objeto)
    {
        objetosEnTeletransporte.Add(objeto); // Marcar objeto como en teletransporte

        // Obtener el collider del objeto y desactivarlo temporalmente
        Collider objCollider = objeto.GetComponent<Collider>();
        if (objCollider != null) objCollider.enabled = false;

        // Guardar la altura actual y teletransportar
        Vector3 nuevaPosicion = otroTeletransportador.transform.position;
        nuevaPosicion.y = objeto.transform.position.y;

        yield return new WaitForSeconds(0.1f); // Pequeña espera antes del teletransporte
        objeto.transform.position = nuevaPosicion;

        yield return new WaitForSeconds(0.3f); // Espera para evitar activación inmediata
        objetosEnTeletransporte.Remove(objeto); // Permitir futuros teletransportes

        // Reactivar el collider del objeto después de un tiempo
        yield return new WaitForSeconds(0.2f);
        if (objCollider != null) objCollider.enabled = true;
    }
}
