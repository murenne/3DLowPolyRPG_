using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotContainer : MonoBehaviour
{
    [SerializeField] private Slot[] _slotArray;
    public Slot[] SlotArray => _slotArray;

    /// <summary>
    /// find and refresh all slots infomation
    /// </summary>
    public void RefreshSlots()
    {
        for (int i = 0; i < SlotArray.Length; i++)
        {
            SlotArray[i].item.ThisItemIndex = i;
            SlotArray[i].UpdateThisSlotItemInfo();
        }
    }
}
