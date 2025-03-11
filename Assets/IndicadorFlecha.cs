using UnityEngine;

public class IndicadorFlecha : MonoBehaviour
{
    public Transform player; // Referencia al cubo (jugador)
    public Transform enemy;  // Referencia al enemigo
    private Vector3 offset;  // Posición relativa inicial de la flecha respecto al cubo

    void Start()
    {
        // Ajustar la flecha para que siempre esté en la cara delantera del cubo
        offset = -player.forward * 1.5f; // Cambia de player.forward a -player.forward
        transform.position = player.position + offset;
    }

    void Update()
    {
        if (enemy != null)
        {
            // Calcular la nueva posición de la flecha en la cara frontal del cubo
            transform.position = player.position + Quaternion.Euler(0, player.rotation.eulerAngles.y, 0) * offset;

            // Obtener la dirección hacia el enemigo
            Vector3 direction = enemy.position - transform.position;
            direction.y = 0; // Mantener la rotación solo en el plano horizontal

            if (direction.magnitude > 0.1f)
            {
                // Mantener la rotación X en 90° y hacer que la flecha apunte al enemigo
                transform.rotation = Quaternion.Euler(90, Quaternion.LookRotation(direction).eulerAngles.y, 0);
            }
        }
    }
}















