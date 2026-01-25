using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Item))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Item _currentItem;
    Slot _currentSlot;
    Slot _targetSlot;

    void Awake()
    {
        // get info itself and slot info
        _currentItem = GetComponent<Item>();
        _currentSlot = GetComponentInParent<Slot>();
    }

    /// <summary>
    /// when begin drag (mouse down)
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // record item's original data
        InventoryManager.Instance.CurrentDragItemInfo = new InventoryManager.DragItemInfo();
        InventoryManager.Instance.CurrentDragItemInfo.originalHolder = GetComponentInParent<Slot>();
        InventoryManager.Instance.CurrentDragItemInfo.originalParent = (RectTransform)transform.parent;

        // reset parent
        transform.SetParent(InventoryManager.Instance.DragCanvas.transform, true);
    }

    /// <summary>
    /// when drag (mouse move)
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        // follow mouse position
        transform.position = eventData.position;
    }

    /// <summary>
    /// when put item (mouse up)
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // point to ui 
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // if mouse in inventory ui area
            if (InventoryManager.Instance.CheckMouseInInventoryUIArea(InventoryManager.Instance.ActionBarInventoryContainer, eventData.position)
                || InventoryManager.Instance.CheckMouseInInventoryUIArea(InventoryManager.Instance.EquipmentInventoryContainer, eventData.position)
                || InventoryManager.Instance.CheckMouseInInventoryUIArea(InventoryManager.Instance.BagInventoryContainer, eventData.position))
            {
                // find slot that mouse position in
                if (eventData.pointerEnter.GetComponent<Slot>())
                    _targetSlot = eventData.pointerEnter.GetComponent<Slot>();
                else
                    _targetSlot = eventData.pointerEnter.GetComponentInParent<Slot>();

                // if diffience from original slot 
                if (_targetSlot != InventoryManager.Instance.CurrentDragItemInfo.originalHolder)
                {
                    // exchange item by slot type
                    switch (_targetSlot.SlotType)
                    {
                        case SlotType.BAG:
                            {
                                SwapItem();
                                break;
                            }
                        case SlotType.WEAPON:
                            {
                                if (_currentItem.ThisInventoryAllData.itemsInfo[_currentItem.ThisItemIndex].itemData.itemType == ItemType.WEAPON)
                                {
                                    SwapItem();
                                }
                                break;
                            }
                        case SlotType.ARMOR:
                            {
                                if (_currentItem.ThisInventoryAllData.itemsInfo[_currentItem.ThisItemIndex].itemData.itemType == ItemType.ARMOR)
                                {
                                    SwapItem();
                                }
                                break;
                            }
                        case SlotType.ACTION:
                            {
                                if (_currentItem.ThisInventoryAllData.itemsInfo[_currentItem.ThisItemIndex].itemData.itemType == ItemType.USEABLE)
                                {
                                    SwapItem();
                                }
                                break;
                            }
                    }
                }

                // reflash two slot info
                _currentSlot.UpdateThisSlotItemInfo();
                _targetSlot.UpdateThisSlotItemInfo();
            }
        }

        // setup dragged item's parent to original 
        transform.SetParent(InventoryManager.Instance.CurrentDragItemInfo.originalParent);

        // resize 
        RectTransform rectTransform = transform as RectTransform;
        rectTransform.offsetMax = -Vector2.one * 5;
        rectTransform.offsetMin = Vector2.one * 5;
    }

    /// <summary>
    /// exchange item data
    /// </summary>
    public void SwapItem()
    {
        // get target slot's item data and dragged slot's item data
        var targetItem = _targetSlot.item.ThisInventoryAllData.itemsInfo[_targetSlot.item.ThisItemIndex];
        var _currentItem = _currentSlot.item.ThisInventoryAllData.itemsInfo[_currentSlot.item.ThisItemIndex];

        // same item ?
        bool isSameItem = _currentItem.itemData == targetItem.itemData;
        if (isSameItem && targetItem.itemData.isStackable)
        {
            // same item and stackable 
            targetItem.itemAmount += _currentItem.itemAmount;

            // clean original slot's item data
            _currentItem.itemData = null;
            _currentItem.itemAmount = 0;
        }
        else
        {
            // exchange item data (because it's a scriptable object, all data will be setup)
            _currentSlot.item.ThisInventoryAllData.itemsInfo[_currentSlot.item.ThisItemIndex] = targetItem;
            _targetSlot.item.ThisInventoryAllData.itemsInfo[_targetSlot.item.ThisItemIndex] = _currentItem;
        }
    }
}
