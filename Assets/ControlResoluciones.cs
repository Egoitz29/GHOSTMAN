using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ControlResoluciones : MonoBehaviour
{
    public TMP_Dropdown dropdownResoluciones;
    private List<Resolution> resolucionesFiltradas = new List<Resolution>();

    void Start()
    {
        Resolution[] todas = Screen.resolutions;
        dropdownResoluciones.ClearOptions();

        List<string> opciones = new List<string>();
        HashSet<string> resolucionesUnicas = new HashSet<string>();

        int indexActual = 0;

        for (int i = 0; i < todas.Length; i++)
        {
            string textoResolucion = todas[i].width + " x " + todas[i].height;

            if (!resolucionesUnicas.Contains(textoResolucion))
            {
                resolucionesUnicas.Add(textoResolucion);
                opciones.Add(textoResolucion);
                resolucionesFiltradas.Add(todas[i]);

                if (todas[i].width == Screen.currentResolution.width &&
                    todas[i].height == Screen.currentResolution.height)
                {
                    indexActual = resolucionesFiltradas.Count - 1;
                }
            }
        }

        dropdownResoluciones.AddOptions(opciones);
        dropdownResoluciones.value = indexActual;
        dropdownResoluciones.RefreshShownValue();

        dropdownResoluciones.onValueChanged.AddListener(CambiarResolucionSeleccionada);
    }

    public void CambiarResolucionSeleccionada(int index)
    {
        Resolution resolucion = resolucionesFiltradas[index];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }
}


