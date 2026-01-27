using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(ScalePop))]
public class UIHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _hoverScale = 1.2f;

    private ScalePop _scalePop;

    private void Awake()
    {
        _scalePop = GetComponent<ScalePop>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _scalePop.ScaleTo(_hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _scalePop.ScaleToOriginal();
    }
}
