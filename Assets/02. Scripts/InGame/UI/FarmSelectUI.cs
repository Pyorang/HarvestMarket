using UnityEngine;
using UnityEngine.UI;

public class FarmSelectUI : MonoBehaviour
{
    [SerializeField] private Button _previousButton;
    [SerializeField] private Button _nextButton;

    private const string FarmChangeSound = "Farm";

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
        string targetSound = $"{FarmChangeSound}{index}";
        AudioManager.Instance.Play(AudioType.SFX, targetSound);

        _previousButton.gameObject.SetActive(!isAtMin);
        _nextButton.gameObject.SetActive(!isAtMax);
    }
}
