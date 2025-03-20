using UnityEngine;
using System.Collections;

public class SfereInteractive : MonoBehaviour
{
    public float velocidadRotacion = 50f;
    public float distanciaCambioColor = 3f;
    public Material materialNormal;
    public Material materialCerca;
    public float tiempoDesaparicion = 0.5f;

    private Renderer rend;
    private bool estaDesapareciendo = false;
    public static Transform enemy;
    private Material materialActual;
    private float tiempoUltimaComprobacion = 0f;
    private float intervaloComprobacion = 0.2f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        materialActual = materialNormal;

        if (materialNormal != null)
        {
            rend.sharedMaterial = materialNormal;
        }
    }
    void Update()
    {
        // 🔄 Rotar la esfera constantemente
        transform.Rotate(0, velocidadRotacion * Time.deltaTime, 0, Space.Self);
    }

    void OnEnable()
    {
        if (enemy == null) return;
        rend.sharedMaterial = materialNormal;
    }

    void FixedUpdate()
    {
        if (enemy == null) return; // ❌ No hacer cálculos innecesarios

        if (Time.time - tiempoUltimaComprobacion > intervaloComprobacion)
        {
            tiempoUltimaComprobacion = Time.time;
            float distancia = Vector3.Distance(transform.position, enemy.position);
            Material nuevoMaterial = (distancia <= distanciaCambioColor) ? materialCerca : materialNormal;

            if (rend.sharedMaterial != nuevoMaterial)
            {
                rend.sharedMaterial = nuevoMaterial;
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
        Color colorInicial = rend.sharedMaterial.color;
        while (tiempo < tiempoDesaparicion)
        {
            float alpha = Mathf.Lerp(1, 0, tiempo / tiempoDesaparicion);
            Color newColor = new Color(colorInicial.r, colorInicial.g, colorInicial.b, alpha);
            rend.sharedMaterial.color = newColor;
            tiempo += Time.deltaTime;
            yield return null;
        }

        Cubepool.Instance.ReturnCube(gameObject); // 🏆 Usar Pooling en lugar de SetActive(false)
        estaDesapareciendo = false;
    }
}
