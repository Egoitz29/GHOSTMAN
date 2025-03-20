using System.Collections;
using UnityEngine;
using TMPro;

public class InicioCuentaAtras : MonoBehaviour
{
    public TMP_Text mensajeTexto; // Texto donde se mostrará la cuenta atrás
    public movimiemtrun playerMovimiento; // Referencia al script de movimiento del Player
    public Temporizador temporizador; // 🔥 Nueva referencia al temporizador

    void Start()
    {
        StartCoroutine(ContadorRegresivo());
    }

    IEnumerator ContadorRegresivo()
    {
        // 🔴 Desactivar el movimiento del Player y el Temporizador al inicio
        if (playerMovimiento != null)
        {
            playerMovimiento.enabled = false;
        }

        if (temporizador != null)
        {
            temporizador.enabled = false;
        }

        // 🕒 Contador de 3 a 1
        for (int i = 3; i > 0; i--)
        {
            mensajeTexto.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // 🏆 Mostrar "¡A por él!"
        mensajeTexto.text = "¡A por el!";
        yield return new WaitForSeconds(1f);

        // ✅ Activar el movimiento del Player
        if (playerMovimiento != null)
        {
            playerMovimiento.enabled = true;
        }

        // ✅ Iniciar el Temporizador
        if (temporizador != null)
        {
            temporizador.enabled = true;
            temporizador.StartCoroutine("ContarTiempo"); // 🔥 Inicia el temporizador manualmente
        }

        // 🔥 Ocultar el mensaje después de empezar el juego
        mensajeTexto.gameObject.SetActive(false);
    }
}
