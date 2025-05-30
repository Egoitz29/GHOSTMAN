using UnityEngine;

public class CambiarCanvas : MonoBehaviour
{
    public Canvas canvasAMostrar;
    public Canvas canvasAOcultar1;
    public Canvas canvasAOcultar2;
   

    public void Cambiar()
    {
        if (canvasAMostrar != null) canvasAMostrar.enabled = true;

        if (canvasAOcultar1 != null) canvasAOcultar1.enabled = false;
        if (canvasAOcultar2 != null) canvasAOcultar2.enabled = false;
        
    }
}
