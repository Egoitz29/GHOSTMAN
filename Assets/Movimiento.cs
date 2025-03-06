using UnityEngine;

public class Movimiento : MonoBehaviour

{
    public float speedEnemy = 5f;
    public float rayDistance = 0.6f;
    public Transform detectorFrontal;
    public string paredTag = "pared";
    public float radioDetector = 0.3f;

    private Rigidbody rb;
    private bool puedeGirar = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * speedEnemy * Time.fixedDeltaTime);
        DetectarParedFrontal();
    }

    void DetectarParedFrontal()
    {
        RaycastHit hit;

        // Siempre usa transform.forward claramente
        if (Physics.SphereCast(detectorFrontal.position, radioDetector, transform.forward, out hit, rayDistance))
        {
            if (hit.collider.CompareTag(paredTag) && puedeGirar)
            {
                Girar90GradosAleatorio();
            }
        }
    }

    void Girar90GradosAleatorio()
    {
        float anguloGiro = Random.value < 0.5f ? -90f : 90f;
        transform.Rotate(0, anguloGiro, 0);
        puedeGirar = false;
        Invoke("RestaurarGiro", 0.2f);
    }

    void RestaurarGiro()
    {
        puedeGirar = true;
    }

    void OnDrawGizmos()
    {
        if (detectorFrontal != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(detectorFrontal.position + transform.forward * rayDistance, radioDetector);
        }
    }
}











