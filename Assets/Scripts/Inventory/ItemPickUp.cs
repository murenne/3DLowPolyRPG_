using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
  public ItemData_SO itemData;
  void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      //if (Input.GetKeyDown(KeyCode.F))
      {
        InventoryManager.Instance.RuntimeBagInventoryData.AddItems(itemData, itemData.itemAmount);
        InventoryManager.Instance.BagInventoryContainer.RefreshSlots();
        Destroy(gameObject);
      }
    }
  }
}
