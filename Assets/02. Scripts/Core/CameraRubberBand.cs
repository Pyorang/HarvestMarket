using UnityEngine;
using DG.Tweening;

public class CameraRubberBand : MonoBehaviour
{
    [Header("고무줄 효과 설정")]
    [SerializeField] private float _pullBackDistance = 0.5f;
    [SerializeField] private float _pullBackDuration = 0.15f;
    [SerializeField] private float _shootDuration = 0.4f;
    [SerializeField] private int _vibrato = 3;
    [SerializeField] private float _elasticity = 0.5f;

    private Sequence _sequence;

    public void Shoot(Transform target)
    {
        _sequence?.Kill();

        Vector3 targetPosition = target.position;
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 pullBackPosition = transform.position - direction * _pullBackDistance;

        _sequence = DOTween.Sequence();

        _sequence.Append(
            transform.DOMove(pullBackPosition, _pullBackDuration)
                .SetEase(Ease.OutQuad)
        );

        _sequence.Append(
            transform.DOMove(targetPosition, _shootDuration)
                .SetEase(Ease.OutElastic, _elasticity, _vibrato)
        );

        _sequence.OnComplete(() => transform.position = targetPosition);
    }

    private void OnDisable()
    {
        _sequence?.Kill();
    }
}
