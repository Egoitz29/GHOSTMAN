using UnityEngine;
using TMPro;  // Importante para usar TextMeshProUGUI

public class ContadorEsferas : MonoBehaviour
{
    public TextMeshProUGUI textoUI;  // Asigna el TextMeshProUGUI desde el Canvas
    private int totalEsferas;

    private void Start()
    {
        // Cuenta las esferas al inicio
        totalEsferas = GameObject.FindGameObjectsWithTag("Esfera").Length;
        ActualizarTexto();
    }

    // Esta función se llama cuando una esfera se desactiva
    public void RestarEsfera()
    {
        totalEsferas--;
        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        textoUI.text = "Esferas restantes: " + totalEsferas;
    }
}
