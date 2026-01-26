using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastPointerHandler : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 touchPosition = eventData.position;

        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("맞은 물체: " + hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("공중에 터치함");
        }
    }
}
