using System.Collections;
using UnityEngine;

public class AnimalAnimation : MonoBehaviour
{
    [SerializeField] private float _maxThinkingTime = 10f;

    private Animator _animator;
    private WaitForSeconds thinkingTime;

    private void Awake()
    {
        float randomThinkingTime = Random.Range(1f, _maxThinkingTime);
        thinkingTime = new WaitForSeconds(randomThinkingTime);
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        TriggerEatingAnimation();
    }

    private void TriggerEatingAnimation()
    {
        StartCoroutine(EatingAnimatonRandomStart());
    }

    private IEnumerator EatingAnimatonRandomStart()
    {
        yield return thinkingTime;

        _animator.SetTrigger("Eat");
    }
}