using TMPro;
using UnityEngine;
using System.Collections;

public class Temporizador : MonoBehaviour
{
    public float tiempoPartida = 60f;
    public TMP_Text tiempoRestanteTexto;
    public float tiempoEsperaAntesDeCerrar = 3f;
    [SerializeField] private GameObject canvasFinal;  // El mismo PanelFinal que usas en MovimientoNavMesh


    private bool tiempoAgotado = false;

    void Update()
    {
        if (tiempoAgotado) return;

        tiempoPartida -= Time.deltaTime;

        if (tiempoPartida <= 0f)
        {
            tiempoPartida = 0f;
            StartCoroutine(FinDelTiempo());
        }

        ActualizarTiempoUI();
    }

    public void AñadirTiempo(float cantidad)
    {
        if (tiempoAgotado) return;

        tiempoPartida += cantidad;
        ActualizarTiempoUI();
    }

    public void RestarTiempo(float cantidad)
    {
        if (tiempoAgotado) return;

        tiempoPartida -= cantidad;

        if (tiempoPartida <= 0f)
        {
            tiempoPartida = 0f;
            StartCoroutine(FinDelTiempo());
        }

        ActualizarTiempoUI();
    }

    private void ActualizarTiempoUI()
    {
        if (tiempoRestanteTexto != null)
        {
            tiempoRestanteTexto.text = "<b>TIEMPO:</b> " + Mathf.CeilToInt(tiempoPartida) + "s";
        }
    }

    private IEnumerator FinDelTiempo()
    {
        if (tiempoAgotado) yield break;

        tiempoAgotado = true;

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

        // ❌ Quitamos el cierre del juego y mostramos el panel
        if (canvasFinal != null)
        {
            canvasFinal.SetActive(true);
        }
        else
        {
            Debug.LogWarning("⚠️ No se ha asignado el Panel Final en el inspector del Temporizador.");
        }
    }
}

