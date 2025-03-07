using UnityEngine;
using System.Collections;

public class SfereInteractive : MonoBehaviour
{
    public float velocidadRotacion = 50f; // Velocidad de rotación
    public float distanciaCambioColor = 3f; // Distancia a la que cambia de color
    public Material materialNormal; // Material base
    public Material materialCerca; // Material cuando el enemigo está cerca
    public float tiempoDesaparicion = 0.5f; // Tiempo en que desaparece la esfera

    private Renderer rend;
    private bool estaDesapareciendo = false;
    private static Transform enemy; // Referencia global al enemigo

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Asegurar que hay materiales asignados
        if (materialNormal != null)
        {
            rend.material = materialNormal;
        }

        // Usar referencia global al enemigo para evitar buscar en cada objeto
        if (enemy == null)
        {
            GameObject enemyObj = GameObject.FindGameObjectWithTag("enemy");
            if (enemyObj != null)
            {
                enemy = enemyObj.transform;
            }
        }
    }

    void FixedUpdate()
    {
        // 🔄 Rotar sobre su propio eje sin que la rotación del padre afecte
        transform.localRotation *= Quaternion.Euler(0, velocidadRotacion * Time.fixedDeltaTime, 0);

        // Cambiar material solo si realmente ha cambiado
        if (enemy != null && materialCerca != null && materialNormal != null)
        {
            float distancia = Vector3.Distance(transform.position, enemy.position);
            bool estaCerca = distancia <= distanciaCambioColor;

            if (estaCerca && rend.material != materialCerca)
            {
                rend.material = materialCerca; // Solo cambia si es necesario
            }
            else if (!estaCerca && rend.material != materialNormal)
            {
                rend.material = materialNormal; // Solo cambia si es necesario
            }
        }
    }

    public void Desaparecer()
    {
        if (!estaDesapareciendo)
        {
            estaDesapareciendo = true;
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        float tiempo = 0;
        Color colorInicial = rend.material.color;

        while (tiempo < tiempoDesaparicion)
        {
            float alpha = Mathf.Lerp(1, 0, tiempo / tiempoDesaparicion);
            Color newColor = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);
            rend.material.color = newColor;
            tiempo += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false); // Desactivar en lugar de destruir para mejor rendimiento
        estaDesapareciendo = false; // Permitir reutilización
    }
}

