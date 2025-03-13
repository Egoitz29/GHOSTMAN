using UnityEngine;

[ExecuteInEditMode] // Permite ver el efecto en el Editor
public class NightVisionEffect : MonoBehaviour
{
    public Material nightVisionMaterial; // Material con el shader
    private bool nightVisionActive = false; // Estado de la visión nocturna

    void Update()
    {
        // Si presionamos la tecla "I", cambiamos el estado de la visión nocturna
        if (Input.GetKeyDown(KeyCode.I))
        {
            nightVisionActive = !nightVisionActive;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (nightVisionActive && nightVisionMaterial != null)
        {
            // Aplica el efecto si está activado
            Graphics.Blit(src, dest, nightVisionMaterial);
        }
        else
        {
            // Renderiza normal si la visión nocturna está desactivada
            Graphics.Blit(src, dest);
        }
    }
}
