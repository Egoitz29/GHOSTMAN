using UnityEngine;

public class EsferasRecolector : MonoBehaviour
{
    private ContadorEsferas contador;

    private void Start()
    {
        // Busca el objeto que tenga el script ContadorEsferas
        contador = FindObjectOfType<ContadorEsferas>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            Debug.Log("💥 Enemy tocó una esfera. Se destruye.");

            if (contador != null)
            {
                contador.RestarEsfera();
            }

            gameObject.SetActive(false);
        }
    }
}
