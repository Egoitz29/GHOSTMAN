using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaTutorial : MonoBehaviour

{
    public string sceneToLoad; // Nombre de la escena a cargar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}


