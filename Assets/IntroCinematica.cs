using UnityEngine;
using System.Collections;

public class IntroCinematica : MonoBehaviour
{
    public Camera cameraIntro;
    public Camera cameraGameplay;
    public Transform puntoInicio;
    public Transform puntoFinal;
    public float duracionMovimiento = 3f;
    public GameObject[] canvasParaOcultar;

    void Start()
    {
        Time.timeScale = 0f; // congela todo el juego
        cameraIntro.enabled = true;
        cameraGameplay.enabled = false;

        // Desactivar todos los canvas indicados
        foreach (GameObject canvas in canvasParaOcultar)
        {
            if (canvas != null)
                canvas.SetActive(false);
        }

        cameraIntro.transform.position = puntoInicio.position;
        cameraIntro.transform.rotation = puntoInicio.rotation;

        StartCoroutine(MoverCamaraIntro());
    }

    IEnumerator MoverCamaraIntro()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duracionMovimiento;
            cameraIntro.transform.position = Vector3.Lerp(puntoInicio.position, puntoFinal.position, t);
            cameraIntro.transform.rotation = Quaternion.Slerp(puntoInicio.rotation, puntoFinal.rotation, t);
            yield return null;
        }

        // Fin de la intro: activar juego normal
        cameraIntro.enabled = false;
        cameraGameplay.enabled = true;

        // Activar los canvas de nuevo
        foreach (GameObject canvas in canvasParaOcultar)
        {
            if (canvas != null)
                canvas.SetActive(true);
        }

        Time.timeScale = 1f;
    }
}
