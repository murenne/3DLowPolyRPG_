using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;
    public bool isStackable;

    [TextArea]
    public string description = "";

    [Header("Weapon")]
    public GameObject weaponPrefab;
    public AttackDataBase_SO weaponAttackData;
    public AnimatorOverrideController weaponAnimator;

    [Header("Useable Item")]
    public UseableItemData_SO useableItemData;
}
