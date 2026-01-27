using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("스케일 설정")]
    [SerializeField] private float _hoverScale = 1.2f;
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private Ease _ease = Ease.OutBack;

    private const string _soundName = "PointerOn";

    private Vector3 _originalScale;
    private Tween _currentTween;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.Play(AudioType.SFX, _soundName);
        _currentTween?.Kill();
        _currentTween = transform.DOScale(_originalScale * _hoverScale, _duration)
            .SetEase(_ease);
    }

    public void OnPointerExit(PointerEventData eventData)
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
