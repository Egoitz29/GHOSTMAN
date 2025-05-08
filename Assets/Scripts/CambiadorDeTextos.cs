using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CambiarTexto : MonoBehaviour
{
    public GameObject textoInicial;
    public GameObject textoFelicidades;
    public GameObject textoAlternativo;
    public GameObject textoExtra;

    public float tiempoFelicidades = 5f;
    public float tiempoFelicidadesFinal = 3f;

    private bool yaCambio = false;
    private bool cicloCompletado = false;
    private bool textoExtraMostrado = false;

    void Start()
    {
        textoInicial.SetActive(true);
        textoFelicidades.SetActive(false);
        textoAlternativo.SetActive(false);
        textoExtra.SetActive(false);
    }

    void Update()
    {
        if (!yaCambio && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W)) && !cicloCompletado)
        {
            yaCambio = true;
            StartCoroutine(MostrarFelicidades());
        }

        if (textoAlternativo.activeSelf && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            textoAlternativo.SetActive(false);
            StartCoroutine(SegundaFelicidadYExtra());
            cicloCompletado = true;
        }

        if (textoExtraMostrado && Input.GetKeyDown(KeyCode.I))
        {
            textoExtra.SetActive(false);
            textoExtraMostrado = false;
        }
    }

    IEnumerator MostrarFelicidades()
    {
        textoInicial.SetActive(false);
        textoFelicidades.SetActive(true);

        yield return new WaitForSeconds(tiempoFelicidades);

        textoFelicidades.SetActive(false);
        textoAlternativo.SetActive(true);
    }

    IEnumerator SegundaFelicidadYExtra()
    {
        textoFelicidades.SetActive(true);

        yield return new WaitForSeconds(tiempoFelicidadesFinal);

        textoFelicidades.SetActive(false);

        // 🔥 Aquí quitamos la espera de 2 segundos
        textoExtra.SetActive(true);
        textoExtraMostrado = true;
    }
}
