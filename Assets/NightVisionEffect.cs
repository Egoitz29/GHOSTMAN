using UnityEngine;

[ExecuteInEditMode] // Permite ver el efecto en el Editor
public class NightVisionEffect : MonoBehaviour
{
    public Material nightVisionMaterial; // Material con el shader
    private bool nightVisionActive = false; // Estado de la visi�n nocturna

    void Update()
    {
        // Si presionamos la tecla "I", cambiamos el estado de la visi�n nocturna
        if (Input.GetKeyDown(KeyCode.I))
        {
            nightVisionActive = !nightVisionActive;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (nightVisionActive && nightVisionMaterial != null)
        {
            // Aplica el efecto si est� activado
            Graphics.Blit(src, dest, nightVisionMaterial);
        }
        else
        {
            // Renderiza normal si la visi�n nocturna est� desactivada
            Graphics.Blit(src, dest);
        }
    }
}
