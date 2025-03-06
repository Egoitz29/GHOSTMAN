using UnityEngine;

public class PlayerMovement : MonoBehaviour

{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float rotationSpeed = 700f; // Velocidad de rotación

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene el Rigidbody del cubo
    }

    void Update()
    {
        // Obtener las entradas de movimiento en el eje horizontal y vertical (teclas de flecha o WASD)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Movimiento hacia adelante y atrás (usando las teclas de dirección o joystick)
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical) * moveSpeed * Time.deltaTime;

        // Mover el cubo
        rb.MovePosition(transform.position + movement);

        // Rotar el cubo para que siempre mire hacia su dirección de movimiento
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}


