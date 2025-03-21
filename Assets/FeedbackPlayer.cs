using UnityEngine;

public class FeedbackPlayer : MonoBehaviour

{
    public CanvasGroup feedbackCanvasGroup; // Este es el grupo que contiene las 4 esquinas
    public float fadeSpeed = 3f;
    private bool mostrar = false;

    void Update()
    {
        float alphaObjetivo = mostrar ? 1f : 0f;
        feedbackCanvasGroup.alpha = Mathf.Lerp(feedbackCanvasGroup.alpha, alphaObjetivo, Time.deltaTime * fadeSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("pared"))
        {
            mostrar = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("pared"))
        {
            mostrar = false;
        }
    }
}

