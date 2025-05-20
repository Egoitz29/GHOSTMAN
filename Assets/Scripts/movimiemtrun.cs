using UnityEngine;

public class movimiemtrun : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 200f;
    private int targetRotation = 0;
    public Temporizador temporizador;

    public float distanciaRaycast = 2f;
    public float offsetAltura = 0.5f; // Ajusta según el tamaño de tu jugador
    public LayerMask capaSuelo;

    void Start()
    {
        targetRotation = Mathf.RoundToInt(transform.eulerAngles.y);
    }

    void Update()
    {
        // PEGADO AL SUELO SIEMPRE
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, distanciaRaycast, capaSuelo))
        {
            if (hit.collider.CompareTag("suelos"))
            {
                Vector3 nuevaPos = transform.position;
                nuevaPos.y = hit.point.y + offsetAltura;
                transform.position = nuevaPos;
            }
        }

        // Movimiento solo si la rotación es exacta
        if (Quaternion.Angle(transform.rotation, Quaternion.Euler(0, targetRotation, 0)) < 1f)
        {
            float moveZ = Input.GetAxis("Vertical");
            Vector3 forwardDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
            transform.position += forwardDirection * moveZ * speed * Time.deltaTime;
        }

        // Rotación
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            targetRotation -= 90;
            if (targetRotation < 0) targetRotation += 360;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            targetRotation += 90;
            if (targetRotation >= 360) targetRotation -= 360;
        }

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, targetRotation, 0),
            turnSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
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

        if (other.CompareTag("poder5"))
        {
            speed += 2;
            Debug.Log("🚀 ¡Velocidad aumentada! Nueva velocidad: " + speed);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("poder2"))
        {
            GameObject[] enemigos = GameObject.FindGameObjectsWithTag("enemy");

            foreach (GameObject enemigo in enemigos)
            {
                UnityEngine.AI.NavMeshAgent agente = enemigo.GetComponent<UnityEngine.AI.NavMeshAgent>();

                if (agente != null)
                {
                    agente.speed = 5f;
                    Debug.Log("🐢 Velocidad del enemigo reducida a 5.");
                }
                else
                {
                    Debug.LogWarning("⚠️ El enemigo no tiene NavMeshAgent asignado.");
                }
            }

            Destroy(other.gameObject);
        }
    }
}
