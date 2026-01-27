using UnityEngine;
using UnityEngine.UI;

public class FarmSelectUI : MonoBehaviour
{
    [SerializeField] private Button _previousButton;
    [SerializeField] private Button _nextButton;

    private void OnEnable()
    {
        FarmManager.OnFarmIndexChanged += HandleFarmIndexChanged;
    }

    private void OnDisable()
    {
        FarmManager.OnFarmIndexChanged -= HandleFarmIndexChanged;
    }

    public void OnPreviousClicked()
    {
        FarmManager.Instance.MoveToPreviousFarm();
    }

    public void OnNextClicked()
    {
        FarmManager.Instance.MoveToNextFarm();
    }

    private void HandleFarmIndexChanged(int index, bool isAtMin, bool isAtMax)
    {
        _previousButton.gameObject.SetActive(!isAtMin);
        _nextButton.gameObject.SetActive(!isAtMax);
    }
}
