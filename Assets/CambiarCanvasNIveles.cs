using UnityEngine;

public class CambiarCanvasNIveles : MonoBehaviour
{
    public GameObject canvasAActivar;
    public GameObject canvasADesactivar1;
    public GameObject canvasADesactivar2;

    public void Cambiar()
    {
        if (canvasAActivar != null) canvasAActivar.SetActive(true);
        if (canvasADesactivar1 != null) canvasADesactivar1.SetActive(false);
        if (canvasADesactivar2 != null) canvasADesactivar2.SetActive(false);
    }
}
