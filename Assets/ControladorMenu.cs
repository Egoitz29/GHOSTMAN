using UnityEngine;

public class ControladorMenu : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject menuOpciones;
    public GameObject menuNiveles;

    public void AbrirMenuPrincipal()
    {
        menuPrincipal.SetActive(true);
        menuOpciones.SetActive(false);
        menuNiveles.SetActive(false);
    }

    public void AbrirMenuOpciones()
    {
        menuPrincipal.SetActive(false);
        menuOpciones.SetActive(true);
        menuNiveles.SetActive(false);
    }

    public void AbrirMenuNiveles()
    {
        menuPrincipal.SetActive(false);
        menuOpciones.SetActive(false);
        menuNiveles.SetActive(true);
    }
}
