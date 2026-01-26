using DG.Tweening;
using UnityEngine;

public class ResourceGainText : MonoBehaviour
{
    [SerializeField] private float _moveDistance = 0.1f;
    [SerializeField] private float _duration = 1f;

    private void OnEnable()
    {
        Play();
    }

    private void Play()
    {
        transform.DOLocalMoveY(transform.localPosition.y + _moveDistance, _duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
