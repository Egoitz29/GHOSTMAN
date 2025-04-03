using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CambiarTexto : MonoBehaviour
{
    public GameObject textoInicial;         // Texto que aparece al principio
    public GameObject textoFelicidades;     // Texto "¡Lo has conseguido!"
    public GameObject textoAlternativo;     // Texto que aparece después

    public float tiempoFelicidades = 5f;    // Duración del mensaje de felicitación
    public float tiempoFelicidadesFinal = 3f; // Tiempo que permanece el texto de felicitaciones después de que se presiona A o D

    private bool yaCambio = false;          // Evitar múltiples cambios
    private bool cicloCompletado = false;   // Controlar el ciclo y evitar que se repita

    void Start()
    {
        textoInicial.SetActive(true);
        textoFelicidades.SetActive(false);
        textoAlternativo.SetActive(false);
    }

    void Update()
    {
        if (!yaCambio && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W)) && !cicloCompletado)
        {
            yaCambio = true;
            StartCoroutine(MostrarFelicidades());
        }

        // Después de que el texto alternativo aparezca, espera una tecla A o D
        if (textoAlternativo.activeSelf && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            textoAlternativo.SetActive(false);  // Desaparece el texto alternativo
            textoFelicidades.SetActive(true);   // Vuelve a mostrar el texto de felicitaciones
            StartCoroutine(EsperarYQuitarFelicidades()); // Espera 3 segundos y luego desaparece el texto de felicitaciones
            cicloCompletado = true;             // Detiene el ciclo, ya no se repite
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

    // Nueva coroutine para esperar 3 segundos antes de quitar el texto de felicitaciones
    IEnumerator EsperarYQuitarFelicidades()
    {
        yield return new WaitForSeconds(tiempoFelicidadesFinal);
        textoFelicidades.SetActive(false);
    }
}
