using UnityEngine;

public class movimiemtrun : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    public float turnSpeed = 200f; // Velocidad de giro (para suavizar)
    private Quaternion targetRotation; // Rotación objetivo
    public Temporizador temporizador; // ✅ Ahora es pública para asignarla desde el Inspector

    void Start()
    {
        targetRotation = transform.rotation; // Inicializar la rotación actual
    }

    void Update()
    {
        float moveZ = Input.GetAxis("Vertical");
        Vector3 forwardDirection = targetRotation * Vector3.forward;
        Vector3 movement = forwardDirection * moveZ * speed * Time.deltaTime;
        transform.position += movement;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            targetRotation *= Quaternion.Euler(0, -90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            targetRotation *= Quaternion.Euler(0, 90, 0);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
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
