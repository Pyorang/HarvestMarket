using DG.Tweening;
using UnityEngine;

public class ResourceGainText : MonoBehaviour
{
    [SerializeField] private float _moveDistance = 0.1f;
    [SerializeField] private float _duration = 1f;

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
