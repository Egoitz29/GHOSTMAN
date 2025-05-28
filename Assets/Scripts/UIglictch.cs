using UnityEngine;
using System.Collections;

public class UIglictch : MonoBehaviour
{
    public GameObject glitchOverlay;

    void Start()
    {
        glitchOverlay.SetActive(false);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("pared"))
        {
            StartCoroutine(MostrarGlitch());
        }
    }

    IEnumerator MostrarGlitch()
    {
        glitchOverlay.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        glitchOverlay.SetActive(false);
    }
}

