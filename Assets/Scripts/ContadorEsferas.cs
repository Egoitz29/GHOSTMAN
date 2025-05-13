using UnityEngine;
using TMPro;

public class ContadorEsferas : MonoBehaviour
{
    public TextMeshProUGUI textoUI;         // Texto en pantalla
    public GameObject canvasFinal;          // Asigna el Canvas a mostrar cuando se acaben las esferas

    private int totalEsferas;

    private void Start()
    {
        totalEsferas = GameObject.FindGameObjectsWithTag("Esfera").Length;
        ActualizarTexto();
        if (canvasFinal != null)
            canvasFinal.SetActive(false);   // Asegura que el canvas esté oculto al inicio
    }

    public void RestarEsfera()
    {
        totalEsferas--;
        ActualizarTexto();

        if (totalEsferas <= 0)
        {
            FinDelJuego();
        }
    }

    private void ActualizarTexto()
    {
        textoUI.text = "Esferas restantes: " + totalEsferas;
    }

    private void FinDelJuego()
    {
        if (canvasFinal != null)
            canvasFinal.SetActive(true);

        Time.timeScale = 0; // Pausa la escena
    }
}
