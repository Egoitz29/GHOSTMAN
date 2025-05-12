using UnityEngine;
using System.Collections;

public class CicloDeInstancia : MonoBehaviour
{
    public GameObject prefab;
    public Transform[] puntosDeInstancia;
    public float duracionInstancia = 3f;
    public float intervaloEntreInstancias = 0.3f;
    public float tiempoDeVidaPrefab = 2f;
    public float tiempoEsperaEntreCiclos = 5f;

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
                InstanciarYDestruir();
                yield return new WaitForSeconds(intervaloEntreInstancias);
                tiempoPasado += intervaloEntreInstancias;
            }

            yield return new WaitForSeconds(tiempoEsperaEntreCiclos);
        }
    }

    void InstanciarYDestruir()
    {
        if (puntosDeInstancia.Length == 0) return;

        int indice = Random.Range(0, puntosDeInstancia.Length);
        Vector3 posicion = puntosDeInstancia[indice].position;

        GameObject obj = Instantiate(prefab, posicion, Quaternion.identity);
        Destroy(obj, tiempoDeVidaPrefab);
    }
}
