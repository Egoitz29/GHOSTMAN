using UnityEngine;

public class ManagerMusic : MonoBehaviour

{
    public AudioClip introClip;
    public AudioClip loopClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayMusicSequence());
    }

    System.Collections.IEnumerator PlayMusicSequence()
    {
        // Reproduce la intro (una vez)
        audioSource.clip = introClip;
        audioSource.loop = false;
        audioSource.Play();

        // Espera a que termine
        yield return new WaitForSeconds(introClip.length);

        // Reproduce el loop (en bucle)
        audioSource.clip = loopClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}

