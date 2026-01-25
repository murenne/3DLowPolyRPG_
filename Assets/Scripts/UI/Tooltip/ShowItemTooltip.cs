using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Item currentItemUI;
    private IGetTooltipCanvas currentCanvas;

    void Awake()
    {
        currentItemUI = GetComponent<Item>();
        currentCanvas = GetComponentInParent<IGetTooltipCanvas>();
    }

    /// <summary>
    /// when mouse position in item ui area
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        currentCanvas.GetItemTooltip().gameObject.SetActive(true);
        currentCanvas.GetItemTooltip().SetupTooltip(currentItemUI.ThisItemData);
    }

    /// <summary>
    /// when mouse position exit slot ui area
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        currentCanvas.GetItemTooltip().gameObject.SetActive(false);
    }

    /// <summary>
    /// when slot off, then tooltip alse off
    /// </summary>
    void OnDisable()
    {
        currentCanvas.GetItemTooltip().gameObject.SetActive(false);
    }
}




