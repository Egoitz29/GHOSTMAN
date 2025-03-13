using UnityEngine;

public class movimiemtrun : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    public float turnSpeed = 200f; // Velocidad de giro
    private int targetRotation = 0; // Ángulo objetivo
    public Temporizador temporizador; // Referencia al temporizador

    void Start()
    {
        targetRotation = Mathf.RoundToInt(transform.eulerAngles.y); // Asegurar que inicie con un ángulo correcto
    }

    void Update()
    {
        // 🔴 No permite moverse hasta que la rotación esté en un ángulo exacto
        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, targetRotation, 0)) < 1f)
        {
            // Movimiento solo si la rotación es exacta
            float moveZ = Input.GetAxis("Vertical");
            Vector3 forwardDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
            transform.position += forwardDirection * moveZ * speed * Time.deltaTime;
        }

        // Rotar a la izquierda
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            targetRotation -= 90;
            if (targetRotation < 0) targetRotation += 360; // Evita valores negativos
        }
        // Rotar a la derecha
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            targetRotation += 90;
            if (targetRotation >= 360) targetRotation -= 360; // Evita valores mayores a 360
        }

        // Aplicar rotación suavemente con RotateTowards
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, targetRotation, 0),
            turnSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        // ✅ Si el Player toca "poder3", suma 30 segundos
        if (other.CompareTag("poder3"))
        {
            Debug.Log("🟢 ¡Player tocó poder3!");

            if (temporizador != null)
            {
                temporizador.AñadirTiempo(30);
                Debug.Log("⏳ Se sumaron 30 segundos: Nuevo tiempo = " + temporizador.tiempoPartida);
            }
            else
            {
                Debug.LogError("❌ El temporizador es NULL, revisa la asignación en el Inspector.");
            }

            Destroy(other.gameObject);
        }

        // ✅ Si el Player toca "poder5", aumenta su velocidad en +5
        if (other.CompareTag("poder5"))
        {
            speed += 5; // 🔥 Aumentar velocidad en +5
            Debug.Log("🚀 ¡Velocidad aumentada! Nueva velocidad: " + speed);

            Destroy(other.gameObject); // 🔥 Eliminar el objeto "poder5"
        }
    }
}
