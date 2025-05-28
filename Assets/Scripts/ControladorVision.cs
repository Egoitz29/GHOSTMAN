using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ControladorVision : MonoBehaviour

{
    public ScriptableRendererFeature nightVisionFeature;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (nightVisionFeature != null)
            {
                nightVisionFeature.SetActive(!nightVisionFeature.isActive);
                Debug.Log("Visión nocturna: " + (nightVisionFeature.isActive ? "ACTIVADA" : "DESACTIVADA"));
            }
        }
    }
}

