using UnityEngine;

public class SalirDelJuego : MonoBehaviour
{
    public void SalirDelJuego1()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Solo para cuando estás en el editor
#else
    Application.Quit(); // Esto funciona en la build final
#endif
    }

}
