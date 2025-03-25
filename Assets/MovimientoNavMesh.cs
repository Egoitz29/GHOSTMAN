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
    public float distanciaHuida = 1f;
    private float tiempoUltimaHuida = -999f;
    public float tiempoEntreHuidas = 0.5f;
    public float velocidadGiro = 500f;

    private int targetRotation = 0;
    private bool isRotating = false;
    private Transform currentWaypoint;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.enabled = false;
        Vector3 posicionDeseada = transform.position;
        posicionDeseada.y = 1;
        transform.position = posicionDeseada;
        agent.enabled = true;

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            agent.Warp(hit.position);
        }
        else
        {
            Debug.LogError("⚠️ No se pudo colocar al enemigo sobre el NavMesh.");
        }

        if (waypoints.Length > 0)
            MoverAlSiguientePunto();
        else
            Debug.LogError("❌ No se han asignado waypoints al enemigo.");
    }


    void Update()
    {
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
            Transform waypointMasLejano = null;
            float mayorDistancia = 0f;

            foreach (Transform wp in waypoints)
            {
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

        Transform nuevoDestino;
        do
        {
            nuevoDestino = waypoints[Random.Range(0, waypoints.Length)];
        } while (nuevoDestino == currentWaypoint && waypoints.Length > 1);

        currentWaypoint = nuevoDestino;

        Vector3 direccion = currentWaypoint.position - transform.position;

        // Elige solo un eje (horizontal o vertical), nunca diagonal
        Vector3 puntoIntermedio = transform.position;
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
            puntoIntermedio += new Vector3(direccion.x, 0, 0); // solo horizontal
        else
            puntoIntermedio += new Vector3(0, 0, direccion.z); // solo vertical

        // Gira claramente antes de avanzar
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.z))
            targetRotation = direccion.x > 0 ? 90 : 270;
        else
            targetRotation = direccion.z > 0 ? 0 : 180;

        isRotating = true;

        // Usa NavMesh para ir solo a ese punto intermedio, evitando diagonales
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
        yield return new WaitForSecondsRealtime(0.1f); // Espera mínima para actualizar Canvas
        Time.timeScale = 0;  // Ahora sí pausa visualmente después de mostrar el mensaje

        yield return new WaitForSecondsRealtime(3f);   // Espera 3 segundos reales con juego pausado
        Time.timeScale = 1;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}