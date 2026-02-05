using UnityEngine;

[RequireComponent(typeof(Farm))]
public class FarmAutoRewarder : MonoBehaviour
{
    private Farm _farm;
    private TextFeedback _textFeedback;
    private static readonly IRewardCalculator s_calculator = new UpgradeRewardCalculator();

    private float _timeElapsed;
    private static readonly float AUTO_INTERVAL = 1.0f;

    private void Awake()
    {
        _farm = GetComponent<Farm>();
        _textFeedback = GetComponent<TextFeedback>();
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed >= AUTO_INTERVAL)
        {
            double reward = s_calculator.CalculateAutoReward(_farm.Currency);

            if (reward > 0)
            {
                CurrencyManager.Instance.AddCurrency(_farm.Currency, reward);

                if (_textFeedback != null)
                {
                    _textFeedback.Play(new ClickInfo(_farm.Currency, reward));
                }
            }

            _timeElapsed = 0f;
        }
    }
}
