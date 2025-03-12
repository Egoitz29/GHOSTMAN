using TMPro;
using UnityEngine;
using System.Collections;

public class Temporizador : MonoBehaviour
{
    public float tiempoPartida = 60f; // ⏳ Ahora es pública para que Unity la actualice en tiempo real
    public TMP_Text tiempoRestanteTexto;
    public float tiempoEsperaAntesDeCerrar = 3f;

    private bool tiempoAgotado = false;

    void Start()
    {
        ActualizarTiempoUI();
        StartCoroutine(ContarTiempo());
    }

    private IEnumerator ContarTiempo()
    {
        while (tiempoPartida > 0)
        {
            yield return new WaitForSeconds(1f);
            tiempoPartida--;
            ActualizarTiempoUI();
        }

        StartCoroutine(FinDelTiempo());
    }

    public void AñadirTiempo(float cantidad)
    {
        tiempoPartida += cantidad; // ✅ Sumar segundos correctamente
        Debug.Log("⏳ Se sumaron " + cantidad + " segundos. Nuevo tiempo: " + tiempoPartida);
        ActualizarTiempoUI();
    }

    public void RestarTiempo(float cantidad)
    {
        tiempoPartida -= cantidad; // ❌ Restar segundos
        Debug.Log("🔴 Se restaron " + cantidad + " segundos. Nuevo tiempo: " + tiempoPartida);

        if (tiempoPartida <= 0)
        {
            tiempoPartida = 0;
            StartCoroutine(FinDelTiempo()); // ✅ Si el tiempo llega a 0, finalizar la partida
        }

        ActualizarTiempoUI();
    }

    private void ActualizarTiempoUI()
    {
        if (tiempoRestanteTexto != null)
        {
            tiempoRestanteTexto.text = "<b>TIEMPO:</b> " + tiempoPartida.ToString("0") + "s";
            Debug.Log("🎮 UI actualizada: " + tiempoRestanteTexto.text);
        }
    }

    private IEnumerator FinDelTiempo()
    {
        if (!tiempoAgotado)
        {
            tiempoAgotado = true;
            Debug.Log("⏳ ¡Se acabó el tiempo!");

            if (tiempoRestanteTexto != null)
            {
                tiempoRestanteTexto.text = "<b>¡TIEMPO AGOTADO!</b>";
                tiempoRestanteTexto.fontSize = 80;
                tiempoRestanteTexto.color = Color.red;
                tiempoRestanteTexto.alignment = TextAlignmentOptions.Center;
                tiempoRestanteTexto.rectTransform.anchoredPosition = new Vector2(0, 200);
            }

            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(tiempoEsperaAntesDeCerrar);
            Time.timeScale = 1;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
