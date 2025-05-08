using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioScene : MonoBehaviour

{
    public void IrAEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }
}


