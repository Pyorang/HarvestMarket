using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public int referenceWidth = 1920;
    public int referenceHeight = 1080;
    private bool isFullscreen = true;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        ApplyResolution();
        ApplyLetterbox();
    }

    private void Update()
    {
        if (Screen.fullScreen != isFullscreen)
        {
            isFullscreen = Screen.fullScreen;
            ApplyResolution();
            ApplyLetterbox();
        }
    }

    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        ApplyResolution();
        ApplyLetterbox();
    }

    private void ApplyResolution()
    {
        if (isFullscreen)
        {
            Resolution nativeRes = Screen.currentResolution;
            Screen.SetResolution(nativeRes.width, nativeRes.height, true);
        }
        else
        {
            Resolution nativeRes = Screen.currentResolution;
            float targetRatio = (float)referenceWidth / referenceHeight;

            int windowHeight = Mathf.RoundToInt(nativeRes.height * 0.8f);
            int windowWidth = Mathf.RoundToInt(windowHeight * targetRatio);

            if (windowWidth > nativeRes.width * 0.9f)
            {
                windowWidth = Mathf.RoundToInt(nativeRes.width * 0.8f);
                windowHeight = Mathf.RoundToInt(windowWidth / targetRatio);
            }

            Screen.SetResolution(windowWidth, windowHeight, false);
        }
    }

    private void ApplyLetterbox()
    {
        if (mainCamera == null) return;

        float targetRatio = (float)referenceWidth / referenceHeight; // 16:9
        float screenRatio = (float)Screen.width / Screen.height;

        if (Mathf.Approximately(screenRatio, targetRatio))
        {
            // 비율 동일 - 전체 화면 사용
            mainCamera.rect = new Rect(0, 0, 1, 1);
        }
        else if (screenRatio > targetRatio)
        {
            // 화면이 더 넓음 - 좌우 검은 띠 (Pillarbox)
            float viewportWidth = targetRatio / screenRatio;
            float x = (1f - viewportWidth) / 2f;
            mainCamera.rect = new Rect(x, 0, viewportWidth, 1);
        }
        else
        {
            // 화면이 더 좁음 - 상하 검은 띠 (Letterbox)
            float viewportHeight = screenRatio / targetRatio;
            float y = (1f - viewportHeight) / 2f;
            mainCamera.rect = new Rect(0, y, 1, viewportHeight);
        }
    }
}