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
        objeto.transform.position = destino.position + Vector3.up * 1.5f;
        yield return new WaitForSeconds(1f);
        enCooldown = false;
    }
}












