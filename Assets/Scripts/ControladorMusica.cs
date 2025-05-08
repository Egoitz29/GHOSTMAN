using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorMusica : MonoBehaviour
{
    public AudioSource musica;
    public Slider sliderVolumen;
    public TextMeshProUGUI textoPorcentaje;

    void Start()
    {
        if (sliderVolumen != null && musica != null)
        {
            sliderVolumen.value = musica.volume;
            ActualizarTextoPorcentaje(musica.volume);
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }
    }

    public void CambiarVolumen(float nuevoVolumen)
    {
        if (musica != null)
        {
            musica.volume = nuevoVolumen;
            ActualizarTextoPorcentaje(nuevoVolumen);
        }
    }

    void ActualizarTextoPorcentaje(float valor)
    {
        if (textoPorcentaje != null)
        {
            int porcentaje = Mathf.RoundToInt(valor * 100f);
            textoPorcentaje.text = porcentaje + "%";
        }
    }
}
