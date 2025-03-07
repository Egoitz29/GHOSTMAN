using UnityEngine;

public class movimiemtrun : MonoBehaviour


{
    public float speed = 5f; // Velocidad de movimiento
    public float turnSpeed = 200f; // Velocidad de giro (para suavizar)
    private Quaternion targetRotation; // Rotaci�n objetivo

    void Start()
    {
        targetRotation = transform.rotation; // Inicializar la rotaci�n actual
    }

    void Update()
    {
        // Movimiento hacia adelante y atr�s (W/S o Flechas Arriba/Abajo)
        float moveZ = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveZ * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // Girar a la izquierda (-90�) o a la derecha (+90�)
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            targetRotation *= Quaternion.Euler(0, -90, 0); // Gira -90�
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            targetRotation *= Quaternion.Euler(0, 90, 0); // Gira +90�
        }

        // Aplicar la rotaci�n de manera suave
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}



