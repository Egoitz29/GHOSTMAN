using UnityEngine;
using TMPro; // Necesario para cambiar el texto si usas TextMeshPro

public class MinimapToggleMenu : MonoBehaviour
{
    public TextMeshProUGUI toggleText;

    private bool isMinimapOn = true;

    void Start()
    {
        isMinimapOn = PlayerPrefs.GetInt("MinimapEnabled", 1) == 1;
        UpdateButtonText();
    }

    public void ToggleMinimapSetting()
    {
        isMinimapOn = !isMinimapOn;
        PlayerPrefs.SetInt("MinimapEnabled", isMinimapOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        toggleText.text = isMinimapOn ? "Minimapa: ENCENDIDO" : "Minimapa: APAGADO";
    }
}
