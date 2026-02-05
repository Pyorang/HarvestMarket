using UnityEngine;

public class FeverSpotLight : MonoBehaviour
{
    private void Awake()
    {
        SetChildrenActive(false);
    }

    private void OnEnable()
    {
        FeverManager.OnFeverStateChanged += OnFeverStateChanged;
    }

    private void OnDisable()
    {
        FeverManager.OnFeverStateChanged -= OnFeverStateChanged;
    }

    private void OnFeverStateChanged(bool isFever)
    {
        SetChildrenActive(isFever);
    }

    private void SetChildrenActive(bool active)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(active);
    }
}
