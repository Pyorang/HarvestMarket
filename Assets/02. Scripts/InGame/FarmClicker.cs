using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Farm))]
public class FarmClicker : MonoBehaviour, IPointerDownHandler
{
    private IClickFeedback[] _feedbacks;

    private void Awake()
    {
        _feedbacks = GetComponents<IClickFeedback>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            foreach (var feedback in _feedbacks)
            {
                feedback.Play();
            }
        }
    }
}
