using UnityEngine;

public class FeverFeedback : MonoBehaviour, IClickFeedback
{
    [SerializeField] private float _gaugeAmount = 1f;

    public void Play(ClickInfo clickInfo)
    {
        FeverManager.Instance.AddGauge(_gaugeAmount);
    }
}
