using UnityEngine;

public class RecolectosEsferaEnemy : MonoBehaviour

{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Esfera"))
        {
            ContadorEsferas.instancia.RestarEsfera();
            Destroy(other.gameObject);
        }
    }
}


