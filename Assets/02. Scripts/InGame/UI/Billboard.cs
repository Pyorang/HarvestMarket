using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        if (_cameraTransform == null) return;

        transform.rotation = _cameraTransform.rotation;
    }
}
