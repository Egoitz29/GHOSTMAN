using UnityEngine;

public class AplicarMaterialGuardado : MonoBehaviour
{
    public Renderer rendererDelFantasma;
    public Material[] materiales;

    void Start()
    {
        int indice = PlayerPrefs.GetInt("MaterialFantasma", 0);

        if (rendererDelFantasma != null && materiales.Length > 0 && indice < materiales.Length)
        {
            rendererDelFantasma.material = materiales[indice];
        }
    }
}
