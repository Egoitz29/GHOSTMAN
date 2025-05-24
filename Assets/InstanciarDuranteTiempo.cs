using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CicloDeInstancia : MonoBehaviour
{
    public GameObject prefab;
    public Transform[] puntosDeInstancia;
    public float duracionInstancia = 3f;
    public float intervaloEntreInstancias = 0.3f;
    public float tiempoDeVidaPrefab = 2f;
    public float tiempoEsperaEntreCiclos = 5f;
    public int maxPrefabsActivos = 3;

    private List<GameObject> prefabsActivos = new List<GameObject>();

    void Start()
    {
        StartCoroutine(Ciclo());
    }

    IEnumerator Ciclo()
    {
        while (true)
        {
            float tiempoPasado = 0f;

            while (tiempoPasado < duracionInstancia)
            {
                InstanciarSiEsPosible();
                yield return new WaitForSeconds(intervaloEntreInstancias);
                tiempoPasado += intervaloEntreInstancias;
            }

            yield return new WaitForSeconds(tiempoEsperaEntreCiclos);
        }
    }

    void InstanciarSiEsPosible()
    {
        LimpiarPrefabsDestruidos(); // Quita los null

        if (prefabsActivos.Count >= maxPrefabsActivos) return;
        if (puntosDeInstancia.Length == 0) return;

        int indice = Random.Range(0, puntosDeInstancia.Length);
        Vector3 posicion = puntosDeInstancia[indice].position;

        GameObject obj = Instantiate(prefab, posicion, Quaternion.identity);
        prefabsActivos.Add(obj);
        Destroy(obj, tiempoDeVidaPrefab);
    }

    void LimpiarPrefabsDestruidos()
    {
        // Elimina los objetos que ya han sido destruidos (null)
        prefabsActivos.RemoveAll(item => item == null);
    }
}
