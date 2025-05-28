using UnityEngine;

public class MinimapLoader : MonoBehaviour
{
    public GameObject minimapCamera;

    void Start()
    {
        bool isEnabled = PlayerPrefs.GetInt("MinimapEnabled", 1) == 1;
        minimapCamera.SetActive(isEnabled);
    }
}
