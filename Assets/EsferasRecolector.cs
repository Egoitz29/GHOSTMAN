using UnityEngine;

public class EsferasRecolector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            Debug.Log("💥 Enemy tocó una esfera. Se destruye.");
            gameObject.SetActive(false);

        }
    }
}

