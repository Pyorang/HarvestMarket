using System.Collections;
using UnityEngine;

public class AnimalAnimation : MonoBehaviour
{
    [SerializeField] private float _maxThinkingTime = 10f;

    [Header("Fever 회전 설정")]
    [SerializeField] private float _feverTurnDuration = 0.5f;
    [SerializeField] private float _turnCooldown = 0.3f;

    private Animator _animator;
    private WaitForSeconds _thinkingTime;
    private WaitForSeconds _cooldownTime;

    private void Awake()
    {
        float randomThinkingTime = Random.Range(1f, _maxThinkingTime);
        _thinkingTime = new WaitForSeconds(randomThinkingTime);
        _cooldownTime = new WaitForSeconds(_turnCooldown);
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        TriggerEatingAnimation();
        FeverManager.OnFeverStateChanged += HandleFeverStateChanged;
    }

    private void OnDisable()
    {
        FeverManager.OnFeverStateChanged -= HandleFeverStateChanged;
    }

    private void TriggerEatingAnimation()
    {
        StartCoroutine(EatingAnimatonRandomStart());
    }

    private IEnumerator EatingAnimatonRandomStart()
    {
        yield return _thinkingTime;
        _animator.SetTrigger("Eat");
    }

    private void HandleFeverStateChanged(bool isFeverActive)
    {
        if (isFeverActive)
        {
            StartCoroutine(FeverTurnRoutine());
        }
    }

    private IEnumerator FeverTurnRoutine()
    {
        while (FeverManager.Instance.IsFeverActive)
        {
            int direction = Random.value > 0.5f ? 1 : -1;
            
            float elapsed = 0f;
            float startY = transform.eulerAngles.y;

            while (elapsed < _feverTurnDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / _feverTurnDuration;
                float currentY = startY + (360f * t * direction);
                transform.rotation = Quaternion.Euler(0f, currentY, 0f);
                yield return null;
            }

            yield return _cooldownTime;
        }
    }
}