using UnityEngine;

public class IndicadorFlecha : MonoBehaviour
{
    public Transform enemy;  // Referencia al enemigo

    void Start()
    {
        // Establece la posición local relativa al cubo (player)
        transform.localPosition = new Vector3(-0.029f, -0.462f, 2.71f);
    }

    void Update()
    {
        if (enemy != null)
        {
            // Obtener la dirección hacia el enemigo en el plano XZ
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
















