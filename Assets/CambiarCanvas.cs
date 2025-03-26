using UnityEngine;

public class CambiarCanvas : MonoBehaviour
{
    public Canvas canvasAMostrar;
    public Canvas canvasAOcultar;

    public void Cambiar()
    {
        if (canvasAMostrar != null) canvasAMostrar.enabled = true;
        if (canvasAOcultar != null) canvasAOcultar.enabled = false;
    }
}
