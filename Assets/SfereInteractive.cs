using UnityEngine;

public class SfereInteractive : MonoBehaviour

{
    public float velocidadRotacion = 50f; // Velocidad de rotación
    public float distanciaCambioColor = 3f; // Distancia a la que cambia de color
    public Material materialNormal; // Material base
    public Material materialCerca; // Material cuando el enemigo está cerca
    public float tiempoDesaparicion = 0.5f; // Tiempo en que desaparece la esfera

    private Renderer rend;
    private Transform enemy;
    private bool estaDesapareciendo = false;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Asegurar que hay materiales asignados
        if (materialNormal != null)
        {
            rend.material = materialNormal;
        }

        // Buscar al enemigo en la escena
        GameObject enemyObj = GameObject.FindGameObjectWithTag("enemy");
        if (enemyObj != null)
        {
            enemy = enemyObj.transform;
        }
    }

    void Update()
    {
        // Hacer que la esfera rote constantemente
        transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime);

        // Si hay un enemigo, cambiar el material según la distancia
        if (enemy != null && materialCerca != null && materialNormal != null)
        {
            float distancia = Vector3.Distance(transform.position, enemy.position);
            rend.material = distancia <= distanciaCambioColor ? materialCerca : materialNormal;
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

    private System.Collections.IEnumerator FadeOut()
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

        Destroy(gameObject);
    }
}


