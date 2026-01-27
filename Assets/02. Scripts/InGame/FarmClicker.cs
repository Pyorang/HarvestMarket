using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Farm))]
public class FarmClicker : MonoBehaviour, IPointerDownHandler
{
    private Camera _mainCamera;
    private Farm _farm;
    private IClickFeedback[] _feedbacks;

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
            double reward = _farm.ClickReward * FeverManager.Instance.BonusMultiplier;
            ClickInfo clickInfo = new ClickInfo(_farm.Resource, reward);

            foreach (var feedback in _feedbacks)
            {
                feedback.Play(clickInfo);
            }

            ResourceManager.Instance.AddResource(_farm.Resource, reward);
        }
    }
}
