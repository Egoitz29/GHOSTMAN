using UnityEngine;

public class IntercambiarCanvas : MonoBehaviour
{
    public GameObject canvasAActivar;
    public GameObject canvasAOcultar;

    public void Intercambiar()
    {
        if (canvasAActivar != null) canvasAActivar.SetActive(true);
        if (canvasAOcultar != null) canvasAOcultar.SetActive(false);
    }
}
