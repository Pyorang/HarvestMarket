using UnityEngine;
using DG.Tweening;

public class ScalePop : MonoBehaviour
{
    [Header("스케일 설정")]
    [SerializeField] private float _popScale = 1.2f;
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private Ease _ease = Ease.OutBack;

    private Vector3 _originalScale;
    private Tween _currentTween;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void Pop()
    {
        _currentTween?.Kill();
        transform.localScale = _originalScale;

        _currentTween = transform.DOScale(_originalScale * _popScale, _duration * 0.5f)
            .SetEase(_ease)
            .OnComplete(() =>
            {
                _currentTween = transform.DOScale(_originalScale, _duration * 0.5f)
                    .SetEase(Ease.OutQuad);
            });
    }

    public void ScaleTo(float scale)
    {
        _currentTween?.Kill();
        _currentTween = transform.DOScale(_originalScale * scale, _duration)
            .SetEase(_ease);
    }

    public void ScaleToOriginal()
    {
        _currentTween?.Kill();
        _currentTween = transform.DOScale(_originalScale, _duration)
            .SetEase(_ease);
    }

    private void OnDisable()
    {
        _currentTween?.Kill();
        transform.localScale = _originalScale;
    }
}
