using UnityEngine;

public class IndicadorFlecha : MonoBehaviour
{
    public Transform enemy;           // Enemigo al que apuntar
    public LayerMask obstaculosMask;  // Capa de obstáculos (estructuras, paredes)
    public GameObject flechaVisual;   // El objeto visual de la flecha (puede ser hijo)

    void Update()
    {
        if (enemy != null)
        {
            // Dirección en plano horizontal
            Vector3 direction = enemy.position - transform.position;
            direction.y = 0;

            if (direction.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.Euler(90, Quaternion.LookRotation(direction).eulerAngles.y, 0);
            }

            // RAYCAST para detectar obstáculos
            Vector3 origen = transform.position;
            Vector3 destino = enemy.position;
            Vector3 direccionRay = destino - origen;

            RaycastHit hit;
            if (Physics.Raycast(origen, direccionRay.normalized, out hit, direccionRay.magnitude, obstaculosMask))
            {
                // Algo está bloqueando la vista → ocultar
                if (flechaVisual != null)
                    flechaVisual.SetActive(false);
            }
            else
            {
                // Vista limpia → mostrar
                if (flechaVisual != null)
                    flechaVisual.SetActive(true);
            }
        }
    }
}









