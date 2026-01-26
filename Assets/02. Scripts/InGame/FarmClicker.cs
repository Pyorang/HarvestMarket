using UnityEngine;
using UnityEngine.EventSystems;

public class FarmClicker : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 touchPosition = eventData.position;

        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 1. 닭들의 크기 커짐
            // 2. 닭 소리 재생
            // 3. 재화 증정
        }
    }
}
