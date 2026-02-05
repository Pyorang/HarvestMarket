using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Farm))]
public class FarmClicker : MonoBehaviour, IPointerDownHandler
{
    private Camera _mainCamera;
    private Farm _farm;
    private IClickFeedback[] _feedbacks;
    private static readonly IRewardCalculator s_calculator = new UpgradeRewardCalculator();

    private void Awake()
    {
        _mainCamera = Camera.main;
        _farm = GetComponent<Farm>();
        _feedbacks = GetComponents<IClickFeedback>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = _mainCamera.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            double reward = s_calculator.CalculateClickReward(_farm.Currency);
            ClickInfo clickInfo = new ClickInfo(_farm.Currency, reward);

            foreach (var feedback in _feedbacks)
            {
                feedback.Play(clickInfo);
            }

            CurrencyManager.Instance.AddCurrency(_farm.Currency, reward);
        }
    }
}
