using DG.Tweening;
using UnityEngine;

public class ScaleFeedback : MonoBehaviour, IClickFeedback
{
    [Header("Scale Settings")]
    [Space]
    [SerializeField] private float _targetScale = 1.2f;
    [SerializeField] private float _scaleUpDuration = 0.15f;
    [SerializeField] private float _scaleDownDuration = 0.2f;

    private Farm _farm;

    private void Awake()
    {
        _farm = GetComponent<Farm>();
    }

    public void Play(ClickInfo clickInfo)
    {
        foreach (var animal in _farm.Animals)
        {
            if (animal.activeSelf)
            {
                animal.transform.DOKill();
                animal.transform.localScale = Vector3.one;

                animal.transform
                    .DOScale(_targetScale, _scaleUpDuration)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                        animal.transform.DOScale(1f, _scaleDownDuration).SetEase(Ease.OutBack));
            }
        }
    }
}
