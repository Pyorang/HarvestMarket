using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Farm))]
public class FarmClicker : MonoBehaviour, IPointerDownHandler
{
    private int _currentSoundNumber = 0;

    [Header("사운드 수")]
    [SerializeField] private int _soundCount = 2;

    [Header("Scale Feedback")]
    [SerializeField] private float _targetScale = 1.2f;
    [SerializeField] private float _scaleUpDuration = 0.15f;
    [SerializeField] private float _scaleDownDuration = 0.2f;

    private string ClickSound = "Clicker";

    private Farm _farm;

    private void Awake()
    {
        _farm = GetComponent<Farm>();
    }

    public void Start()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 touchPosition = eventData.position;

        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            PlayScaleFeedback();
            PlaySoundFeedback();
            // TODO: 재화 획득
        }
    }

    public void PlayScaleFeedback()
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

    public void PlaySoundFeedback()
    {
        /*int targetSoundNumber = ++_currentSoundNumber;
        _currentSoundNumber %= _soundCount;

        string targetSoundName = $"{_farm.AnimalType}{targetSoundNumber}";

        AudioManager.Instance.Play(AudioType.SFX, targetSoundName);*/
        AudioManager.Instance.Play(AudioType.SFX, ClickSound);
    }
}
