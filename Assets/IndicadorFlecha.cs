using UnityEngine;

public class IndicadorFlecha : MonoBehaviour
{
    public Transform player; // Referencia al cubo (jugador)
    public Transform enemy;  // Referencia al enemigo
    private Vector3 offset;  // Posici�n relativa inicial de la flecha respecto al cubo

    void Start()
    {
        // Ajustar la flecha para que siempre est� en la cara delantera del cubo
        offset = -player.forward * 1.5f; // Cambia de player.forward a -player.forward
        transform.position = player.position + offset;
    }

    void Update()
    {
        if (enemy != null)
        {
            // Calcular la nueva posici�n de la flecha en la cara frontal del cubo
            transform.position = player.position + Quaternion.Euler(0, player.rotation.eulerAngles.y, 0) * offset;

            // Obtener la direcci�n hacia el enemigo
            Vector3 direction = enemy.position - transform.position;
            direction.y = 0; // Mantener la rotaci�n solo en el plano horizontal

            if (direction.magnitude > 0.1f)
            {
                // Mantener la rotaci�n X en 90� y hacer que la flecha apunte al enemigo
                transform.rotation = Quaternion.Euler(90, Quaternion.LookRotation(direction).eulerAngles.y, 0);
            }
        }
    }
}















