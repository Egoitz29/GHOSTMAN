using System.Collections;
using UnityEngine;

public class PrefabPoderes : MonoBehaviour
{
    public GameObject[] prefabs; // Lista de prefabs para elegir aleatoriamente
    public Transform[] spawnPositions; // Lista de posiciones donde se pueden instanciar
    public float tiempoMinSpawn = 10f; // Tiempo mínimo antes de instanciar un nuevo prefab
    public float tiempoMaxSpawn = 25f; // Tiempo máximo antes de instanciar un nuevo prefab
    public float tiempoDeDesaparicion = 5f; // Tiempo antes de que el prefab desaparezca

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
            Debug.Log(" Esperando " + tiempoEspera + " segundos para la siguiente instancia...");
            yield return new WaitForSeconds(tiempoEspera);

            // Instanciar si no hay un prefab activo
            if (instanciaActual == null && prefabs.Length > 0 && spawnPositions.Length > 0)
            {
                int prefabIndex = Random.Range(0, prefabs.Length); // Seleccionar un prefab aleatorio
                int positionIndex = Random.Range(0, spawnPositions.Length); // Seleccionar una posición aleatoria

                instanciaActual = Instantiate(prefabs[prefabIndex], spawnPositions[positionIndex].position, Quaternion.identity);

                Debug.Log("Instanciado prefab: " + prefabs[prefabIndex].name + " en posición " + spawnPositions[positionIndex].name);

                // Iniciar temporizador de desaparición
                StartCoroutine(DestruirDespuesDeTiempo(instanciaActual, tiempoDeDesaparicion));
            }
        }
    }

    IEnumerator DestruirDespuesDeTiempo(GameObject objeto, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        if (objeto != null)
        {
            Debug.Log(" Eliminado prefab: " + objeto.name);
            Destroy(objeto);
            instanciaActual = null; // Permitir que se pueda instanciar otro
        }
    }
}



