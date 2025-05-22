using UnityEngine;

public class ControladorMaterial : MonoBehaviour

{
    public Renderer previewRenderer;
    public Material[] materiales;

    private int indiceActual = 0;

    void Start()
    {
        // Recupera la selección anterior si existe
        indiceActual = PlayerPrefs.GetInt("MaterialFantasma", 0);
        AplicarMaterial();
    }

    public void SiguienteMaterial()
    {
        indiceActual++;
        if (indiceActual >= materiales.Length) indiceActual = 0;
        AplicarMaterial();
    }

    public void AnteriorMaterial()
    {
        indiceActual--;
        if (indiceActual < 0) indiceActual = materiales.Length - 1;
        AplicarMaterial();
    }

    private void AplicarMaterial()
    {
        if (previewRenderer != null && materiales.Length > 0)
        {
            previewRenderer.material = materiales[indiceActual];
            PlayerPrefs.SetInt("MaterialFantasma", indiceActual);
        }
    }
}


