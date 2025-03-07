using TMPro;
using UnityEngine;

public class EnemyRecolector : MonoBehaviour
{
    public string tagEsfera = "Esfera";
    public TMP_Text contadorTexto;
    private int esferasRestantes;

    void Start()
    {
        // Comprobación de que hay esferas en la escena
        ActualizarContador();

        // Verificar que el texto esté asignado en el Inspector
        if (contadorTexto == null)
        {
            Debug.LogError("EnemyRecolector: No se ha asignado un objeto TMP_Text al contadorTexto.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagEsfera))
        {
            Destroy(other.gameObject);
            esferasRestantes--;
            ActualizarUI();
        }
    }

    void ActualizarContador()
    {
        esferasRestantes = GameObject.FindGameObjectsWithTag(tagEsfera).Length;
        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (contadorTexto != null)
        {
            contadorTexto.text = "Esferas restantes: " + esferasRestantes;
        }
    }
}
