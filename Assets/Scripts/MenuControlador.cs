using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public void VolverAlMenu()
    {
        Time.timeScale = 1f; // Por si acaso estaba en pausa
        SceneManager.LoadScene("Menu"); // Cambia por el nombre real de tu escena de menú
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
