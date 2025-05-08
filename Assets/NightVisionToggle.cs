using UnityEngine;

public class NightVisionToggle : MonoBehaviour

{
    public Material nightVisionMaterial;
    private bool activarVision = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activarVision = !activarVision;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (activarVision && nightVisionMaterial != null)
        {
            Graphics.Blit(src, dest, nightVisionMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

}
