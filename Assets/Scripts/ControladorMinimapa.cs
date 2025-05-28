using TMPro;
using UnityEngine;

public class ControladorMinimapa : MonoBehaviour

{
    public TextMeshProUGUI textoBoton;

    private bool minimapaActivo = true;

    void Start()
    {
        minimapaActivo = PlayerPrefs.GetInt("MinimapaActivado", 1) == 1;
        ActualizarTexto();
    }

    public void AlternarMinimapa()
    {
        minimapaActivo = !minimapaActivo;
        PlayerPrefs.SetInt("MinimapaActivado", minimapaActivo ? 1 : 0);
        PlayerPrefs.Save();
        ActualizarTexto();
        Debug.Log("Minimapa ahora está: " + (minimapaActivo ? "ACTIVADO" : "DESACTIVADO"));
    }

    void ActualizarTexto()
    {
        textoBoton.text = minimapaActivo ? "Minimapa: Activado" : "Minimapa: Desactivado";
    }
}
