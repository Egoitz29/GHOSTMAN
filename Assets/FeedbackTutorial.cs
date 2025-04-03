using UnityEngine;

public class FeedbackTutorial : MonoBehaviour

{
    public float speed = 5f; // Velocidad de movimiento en Y
    public float maxY = 5f;  // Altura máxima
    public float minY = 0f;  // Altura mínima

    private bool isMovingUp = true; // Dirección de movimiento: hacia arriba o hacia abajo

    void Update()
    {
        // Mover el objeto hacia arriba
        if (isMovingUp)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
            if (transform.position.y >= maxY) // Alcanza la altura máxima, cambiar dirección
            {
                isMovingUp = false;
            }
        }
        // Mover el objeto hacia abajo
        else
        {
            transform.position -= Vector3.up * speed * Time.deltaTime;
            if (transform.position.y <= minY) // Alcanza la altura mínima, cambiar dirección
            {
                isMovingUp = true;
            }
        }
    }
}

