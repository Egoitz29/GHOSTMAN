using System.Collections;
using UnityEngine;
using TMPro;

public class PrefabPoderes : MonoBehaviour
{
    public GameObject[] prefabs; // Lista de prefabs para elegir aleatoriamente
    public Transform[] spawnPositions; // Lista de posiciones donde se pueden instanciar
    public float tiempoMinSpawn = 10f; // Tiempo mínimo antes de instanciar un nuevo prefab
    public float tiempoMaxSpawn = 25f; // Tiempo máximo antes de instanciar un nuevo prefab
    public float tiempoDeDesaparicion = 5f; // Tiempo antes de que el prefab desaparezca

    public TMP_Text mensajeNotificacion; // 🟢 Texto en pantalla para notificar al jugador

    public Material materialAzul; // Material azul
    public Material materialRojo; // Material rojo

    private GameObject instanciaActual; // Referencia al prefab instanciado

    void Start()
    {
        // Iniciar la primera instancia aleatoriamente
        StartCoroutine(SpawnPrefabAleatorio());
    }

    IEnumerator SpawnPrefabAleatorio()
    {
        while (true) // Bucle infinito para que siempre se sigan creando
        {
            // Esperar un tiempo aleatorio antes de instanciar
            float tiempoEspera = Random.Range(tiempoMinSpawn, tiempoMaxSpawn);
            Debug.Log("Esperando " + tiempoEspera + " segundos para la siguiente instancia...");
            yield return new WaitForSeconds(tiempoEspera);

            // Instanciar si no hay un prefab activo
            if (instanciaActual == null && prefabs.Length > 0 && spawnPositions.Length > 0)
            {
                int prefabIndex = Random.Range(0, prefabs.Length); // Seleccionar un prefab aleatorio
                int positionIndex = Random.Range(0, spawnPositions.Length); // Seleccionar una posición aleatoria

                instanciaActual = Instantiate(prefabs[prefabIndex], spawnPositions[positionIndex].position, Quaternion.identity);

                Debug.Log("Instanciado prefab: " + prefabs[prefabIndex].name + " en posición " + spawnPositions[positionIndex].name);

                // 🟢 Determinar el color del texto según el material del prefab
                Color colorTexto = ObtenerColorSegunMaterial(instanciaActual);

                // 🟢 Mostrar mensaje en pantalla con el color adecuado
                NotificarJugador("¡Ha aparecido un nuevo poder: " + prefabs[prefabIndex].name + "!", colorTexto);

                // Iniciar temporizador de desaparición
                StartCoroutine(DestruirDespuesDeTiempo(instanciaActual, tiempoDeDesaparicion));
            }
        }
    }

    // 🟢 Método para mostrar notificación con color dinámico
    void NotificarJugador(string mensaje, Color color)
    {
        if (mensajeNotificacion != null)
        {
            mensajeNotificacion.text = mensaje;
            mensajeNotificacion.color = color; // Cambiar el color del texto
            mensajeNotificacion.gameObject.SetActive(true);

            // Ocultar mensaje después de 3 segundos
            StartCoroutine(DesactivarNotificacion(3f));
        }
    }

    // 🟢 Método para obtener el color según el material del prefab
    Color ObtenerColorSegunMaterial(GameObject objeto)
    {
        Renderer renderer = objeto.GetComponent<Renderer>();

        if (renderer != null)
        {
            Material materialUsado = renderer.sharedMaterial;

            if (materialUsado == materialAzul)
            {
                return Color.blue; // Texto en azul
            }
            else if (materialUsado == materialRojo)
            {
                return Color.red; // Texto en rojo
            }
        }

        return Color.white; // Texto en blanco por defecto
    }

    IEnumerator DesactivarNotificacion(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        if (mensajeNotificacion != null)
        {
            mensajeNotificacion.gameObject.SetActive(false);
        }
    }

    IEnumerator DestruirDespuesDeTiempo(GameObject objeto, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        if (objeto != null)
        {
            Debug.Log("Eliminado prefab: " + objeto.name);
            Destroy(objeto);
            instanciaActual = null; // Permitir que se pueda instanciar otro
        }
    }
}
