using UnityEngine;

[ExecuteInEditMode] // Permite ver el efecto en el Editor
public class NightVisionEffect : MonoBehaviour
{
    public Material nightVisionMaterial; // Material con el shader

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (nightVisionMaterial != null)
        {
            // Aplica siempre el efecto
            Graphics.Blit(src, dest, nightVisionMaterial);
        }
        else
        {
            // Renderiza normal si no hay material asignado
            Graphics.Blit(src, dest);
        }
    }
}
