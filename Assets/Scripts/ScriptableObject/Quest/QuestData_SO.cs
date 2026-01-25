using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire
    {
        public string Name;
        public int requireAmount;
        public int currentAmount;
    }

    public string questName;
    [TextArea]
    public string description;
    public bool isStarted;
    public bool isCompleted;
    public bool isFinished;

    public List<QuestRequire> questRequireItemList = new();
    public List<InventoryItemInfo> questRewardItemList = new();
}
