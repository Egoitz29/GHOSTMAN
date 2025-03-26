using UnityEngine;

public class MusicaEscena : MonoBehaviour
{
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
        {
            audio.Play();
        }
    }
}

