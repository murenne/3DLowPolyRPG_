using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Image _thisItemIcon;
    [SerializeField] private Text _thisItemAmount;
    [SerializeField] private ItemData_SO _thisItemData;
    public ItemData_SO ThisItemData => _thisItemData;
    public int ThisItemIndex { get; set; } = -1;
    public InventoryData_SO ThisInventoryAllData { get; set; }

    /// <summary>
    /// setup item info (data and amount)
    /// </summary>
    public void SetupItemInfo(ItemData_SO itemData, int itemAmount)
    {
        // amout = 0 means there are no items 
        if (itemAmount == 0)
        {
            // avoid error, update all data
            ThisInventoryAllData.itemsInfo[ThisItemIndex].itemData = null;
            _thisItemIcon.gameObject.SetActive(false);
            return;
        }

        // have item data then setup
        if (itemData != null)
        {
            _thisItemData = itemData;
            _thisItemAmount.text = itemAmount.ToString("00");
            _thisItemIcon.sprite = itemData.itemIcon;
            _thisItemIcon.gameObject.SetActive(true);
        }
        else
        {
            _thisItemIcon.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// get item data
    /// </summary>
    public ItemData_SO GetItemData()
    {
        return ThisInventoryAllData.itemsInfo[ThisItemIndex].itemData;
    }
}
