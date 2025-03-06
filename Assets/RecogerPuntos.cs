using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class EnemyRecolector : MonoBehaviour

{
    public string tagEsfera = "Esfera";
    public TMP_Text contadorTexto; // Cambiado a TMP_Text
    private int esferasRestantes;

    void Start()
    {
        esferasRestantes = GameObject.FindGameObjectsWithTag(tagEsfera).Length;
        ActualizarUI();
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

    void ActualizarUI()
    {
        if (contadorTexto != null)
        {
            contadorTexto.text = "Esferas restantes: " + esferasRestantes;
        }
    }
}



