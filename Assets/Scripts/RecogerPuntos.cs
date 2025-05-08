using TMPro;
using UnityEngine;

public class EnemyRecolector : MonoBehaviour
{
    public string tagEsfera = "Esfera";
    public string tagPoder4 = "poder4";
    public string tagPoder6 = "poder6"; // ✅ Nuevo tag para detectar poder6
    public TMP_Text contadorTexto;
    private int esferasRestantes;
    private Temporizador temporizador;
    private movimiemtrun player; // ✅ Referencia al Player

    void Start()
    {
        // Buscar el temporizador en la escena
        temporizador = FindAnyObjectByType<Temporizador>();

        if (temporizador == null)
        {
            Debug.LogError("❌ No se encontró el Temporizador en la escena. Asegúrate de que está activo.");
        }

        // Buscar el Player en la escena
        player = FindAnyObjectByType<movimiemtrun>();

        if (player == null)
        {
            Debug.LogError("❌ No se encontró el Player en la escena. Asegúrate de que tiene el script 'movimiemtrun'.");
        }

        // Comprobación de que hay esferas en la escena
        ActualizarContador();

        // Verificar que el texto esté asignado en el Inspector
        if (contadorTexto == null)
        {
            Debug.LogError("❌ EnemyRecolector: No se ha asignado un objeto TMP_Text al contadorTexto.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // ✅ Si el enemigo toca una esfera, la destruye y actualiza el contador
        if (other.CompareTag(tagEsfera))
        {
            Destroy(other.gameObject);
            esferasRestantes--;
            ActualizarUI();
        }

        // ✅ Si el enemigo toca un "poder4", se restan 30 segundos al temporizador
        if (other.CompareTag(tagPoder4))
        {
            Debug.Log("🔴 ¡Enemy tocó poder4!");

            if (temporizador != null)
            {
                temporizador.RestarTiempo(30); // 🔴 Restar 30 segundos
                Debug.Log("⏳ Se restaron 30 segundos al tiempo.");
            }

            Destroy(other.gameObject); // 🔥 Eliminar el objeto "poder4"
        }

        // ✅ Si el enemigo toca "poder6", reduce la velocidad del Player en -3
        if (other.CompareTag(tagPoder6))
        {
            Debug.Log("⚠️ ¡Enemy tocó poder6!");

            if (player != null)
            {
                player.speed -= 3; // ⏬ Reducir la velocidad en 3
                if (player.speed < 1) player.speed = 1; // Evitar que la velocidad sea 0 o negativa
                Debug.Log("🐢 Velocidad del Player reducida. Nueva velocidad: " + player.speed);
            }

            Destroy(other.gameObject); // 🔥 Eliminar el objeto "poder6"
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
