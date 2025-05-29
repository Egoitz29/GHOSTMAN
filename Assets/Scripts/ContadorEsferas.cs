using UnityEngine;
using TMPro;

public class ContadorEsferas : MonoBehaviour
{
    public static ContadorEsferas instancia;

    public TextMeshProUGUI textoUI;         // Texto en pantalla
    public GameObject canvasFinal;          // Canvas de fin de juego

    private int totalEsferas;

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        totalEsferas = GameObject.FindGameObjectsWithTag("Esfera").Length;
        ActualizarTexto();

        if (canvasFinal != null)
            canvasFinal.SetActive(false);
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

        Time.timeScale = 0f;
    }
}
