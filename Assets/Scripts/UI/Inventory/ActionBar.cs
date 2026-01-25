using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour
{
    [SerializeField] private KeyCode actionKey;
    private Slot _currnetSlot;

    void Awake()
    {
        _currnetSlot = GetComponent<Slot>();
    }

    /// <summary>
    /// use keyboard to use item
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(actionKey) && _currnetSlot.item.GetItemData())
        {
            _currnetSlot.UseItem();
        }
    }
}
