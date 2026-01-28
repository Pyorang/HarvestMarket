using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public int targetWidth = 1920;
    public int targetHeight = 1080;

    private bool isFullscreen = true;

    private void Awake()
    {
        Screen.SetResolution(targetWidth, targetHeight, isFullscreen);
    }
}