using UnityEngine;

public class Teleport : MonoBehaviour
{
  
    public Transform cubo1; // Referencia al primer cubo
    public Transform cubo2; // Referencia al segundo cubo

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el player tiene la etiqueta "Player"
        {
            if (transform == cubo1)
            {
                other.transform.position = cubo2.position + Vector3.up * 1.5f; // Teletransportar al cubo2
            }
            else if (transform == cubo2)
            {
                other.transform.position = cubo1.position + Vector3.up * 1.5f; // Teletransportar al cubo1
            }
        }
    }
}

