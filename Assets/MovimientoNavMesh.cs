using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class MovimientoNavMesh : MonoBehaviour
{
    public Transform[] waypoints;
    private NavMeshAgent agent;
    public TMP_Text mensajeFinalizacion;
    public float tiempoEsperaAntesDeCerrar = 3f;
    public static List<MovimientoNavMesh> enemigos = new List<MovimientoNavMesh>();
    public GameObject player;
    public TMP_Text mensajeCanvas;
    public float distanciaHuida = 999f;
    private float tiempoUltimaHuida = -999f;
    public float tiempoEntreHuidas = 0.5f;
    public float velocidadGiro = 500f;

    private int targetRotation = 0;
    private bool isRotating = false;
    private Transform currentWaypoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // ✅ Deja que el NavMeshAgent controle rotación y altura
        agent.updateRotation = true;
        agent.updateUpAxis = true;

        // 🔧 Asegúrate de que está activado
        agent.enabled = true;

        if (waypoints.Length > 0)
            MoverAlSiguientePunto();
        else
            Debug.LogError("❌ No se han asignado waypoints al enemigo.");
    }

    void Update()
    {
        // 🛡 Protección total: si se perdió la referencia, intenta recuperarla
        if (player == null)
        {
            if (player == null)
            {
                Debug.LogWarning("🚫 El objeto player sigue siendo null");
                return;
            }

            GameObject encontrado = GameObject.Find("ghost"); // 👈 busca por nombre
            if (encontrado != null)
            {
                player = encontrado;
                Debug.Log("♻️ Se reasignó el player al objeto llamado 'ghost'.");
            }
            else
            {
                // ⚠️ Si no lo encuentra, salimos sin hacer nada
                Debug.LogWarning("❌ No se encontró el objeto 'ghost' en escena.");
                return;
            }
        }

        if (isRotating)
        {
            RotarHaciaObjetivo();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoverAlSiguientePunto();
        }

        float distancia = Vector3.Distance(transform.position, player.transform.position);
        



        if (distancia < distanciaHuida && Time.time - tiempoUltimaHuida > tiempoEntreHuidas)
        {
            Debug.Log("⚠️ Modo huida activado");
            Transform waypointMasLejano = null;
            float mayorDistancia = 0f;

            foreach (Transform wp in waypoints)
            {
                if (wp == null) continue;

                float d = Vector3.Distance(wp.position, player.transform.position);
                if (d > mayorDistancia)
                {
                    mayorDistancia = d;
                    waypointMasLejano = wp;
                }
            }

            if (waypointMasLejano != null)
            {
                currentWaypoint = waypointMasLejano;
                Vector3 direccion = (currentWaypoint.position - transform.position).normalized;

                if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
                    targetRotation = direccion.x > 0 ? 90 : 270;
                else
                    targetRotation = direccion.z > 0 ? 0 : 180;

                isRotating = true;
                tiempoUltimaHuida = Time.time;
            }
        }

        ActualizarRotacionVisual();
    }


    void RotarHaciaObjetivo()
    {
        Quaternion rotObjetivo = Quaternion.Euler(0, targetRotation, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotObjetivo, velocidadGiro * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, rotObjetivo) < 1f)
        {
            transform.rotation = rotObjetivo;
            isRotating = false;
            agent.SetDestination(currentWaypoint.position);
        }
    }

    public void MoverAlSiguientePunto()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("⚠️ No hay waypoints asignados.");
            return;
        }

        // ✅ Filtrar los waypoints válidos
        List<Transform> waypointsValidos = new List<Transform>();
        foreach (Transform wp in waypoints)
        {
            if (wp != null) waypointsValidos.Add(wp);
        }

        if (waypointsValidos.Count == 0)
        {
            Debug.LogError("❌ Todos los waypoints han sido destruidos o son nulos.");
            return;
        }

        Transform nuevoDestino;
        do
        {
            nuevoDestino = waypointsValidos[Random.Range(0, waypointsValidos.Count)];
        } while (nuevoDestino == currentWaypoint && waypointsValidos.Count > 1);

        currentWaypoint = nuevoDestino;

        Vector3 direccion = currentWaypoint.position - transform.position;

        Vector3 puntoIntermedio = transform.position;
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
            puntoIntermedio += new Vector3(direccion.x, 0, 0);
        else
            puntoIntermedio += new Vector3(0, 0, direccion.z);

        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
            targetRotation = direccion.x > 0 ? 90 : 270;
        else
            targetRotation = direccion.z > 0 ? 0 : 180;

        isRotating = true;

        agent.SetDestination(puntoIntermedio);
    }

    void ActualizarRotacionVisual()
    {
        Vector3 direccionMovimiento = agent.velocity.normalized;

        if (direccionMovimiento.magnitude > 0.1f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadGiro * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("enemy"))
        {
            if (mensajeCanvas != null)
            {
                mensajeCanvas.text = "¡Enhorabuena crack, los fantasmas han ganado!";
                mensajeCanvas.gameObject.SetActive(true);
                mensajeCanvas.ForceMeshUpdate();
            }

            StartCoroutine(ReanudarTiempoYCerrar());
        }
    }

    IEnumerator ReanudarTiempoYCerrar()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
