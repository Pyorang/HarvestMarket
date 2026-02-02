using UnityEngine;
using DG.Tweening;

public class SpotLightEffect : MonoBehaviour
{
    [Header("회전 범위")]
    [SerializeField] private float _minRotationX = -30f;
    [SerializeField] private float _maxRotationX = 30f;
    [SerializeField] private float _minRotationY = -30f;
    [SerializeField] private float _maxRotationY = 30f;

    [Header("회전 속도")]
    [SerializeField] private float _rotationDuration = 0.3f;

    private Transform[] _lightTransforms;
    private Vector3[] _initialRotations;
    private Tween[] _tweens;

    private void Awake()
    {
        int childCount = transform.childCount;
        _lightTransforms = new Transform[childCount];
        _initialRotations = new Vector3[childCount];
        _tweens = new Tween[childCount];
        
        for (int i = 0; i < childCount; i++)
        {
            _lightTransforms[i] = transform.GetChild(i);
            _initialRotations[i] = _lightTransforms[i].localEulerAngles;
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < _lightTransforms.Length; i++)
        {
            StartRandomRotation(i);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _tweens.Length; i++)
        {
            _tweens[i]?.Kill();
            _tweens[i] = null;
        }
    }

    private void StartRandomRotation(int index)
    {
        Vector3 baseRotation = _initialRotations[index];
        float randomX = baseRotation.x + Random.Range(_minRotationX, _maxRotationX);
        float randomY = baseRotation.y + Random.Range(_minRotationY, _maxRotationY);
        Vector3 targetRotation = new Vector3(randomX, randomY, baseRotation.z);

        _tweens[index]?.Kill();
        _tweens[index] = _lightTransforms[index]
            .DOLocalRotate(targetRotation, _rotationDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => StartRandomRotation(index));
    }
}
