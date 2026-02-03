using UnityEngine;
using UnityEngine.UI;

public class FeverGaugeUI : MonoBehaviour
{
    [SerializeField] private Slider _gaugeSlider;

    private void Awake()
    {
        FeverManager.OnGaugeChanged += UpdateGauge;
    }

    private void UpdateGauge(float current, float max)
    {
        _gaugeSlider.value = current / max;
    }

    private void OnDisable()
    {
        FeverManager.OnGaugeChanged -= UpdateGauge;
    }
}
