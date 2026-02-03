using DG.Tweening;
using TMPro;
using UnityEngine;

public class ResourceGainText : MonoBehaviour
{
    [Header("이동 설정")]
    [Space]
    [SerializeField] private float _moveDistance = 0.1f;
    [SerializeField] private float _duration = 1f;

    [Header("텍스트")]
    [Space]
    [SerializeField] private TextMeshPro _gainText;

    private Vector3 _initialLocalPosition;
    private GameObject _spawnedIcon;

    private void Awake()
    {
        _initialLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        Play();
    }

    public void SetGainText(double amount)
    {
        _gainText.text = $"+{amount.ToFormattedString()}";
    }

    public void SetIcon(GameObject icon)
    {
        _spawnedIcon = icon;
    }

    private void Play()
    {
        transform.DOLocalMoveY(transform.localPosition.y + _moveDistance, _duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                DespawnIcon();
                TextFeedbackPool.Instance.Despawn(gameObject);
            });
    }

    private void DespawnIcon()
    {
        if (_spawnedIcon != null)
        {
            TextFeedbackPool.Instance.Despawn(_spawnedIcon);
            _spawnedIcon = null;
        }
    }

    private void OnDisable()
    {
        transform.DOKill();
        transform.localPosition = _initialLocalPosition;
    }
}
