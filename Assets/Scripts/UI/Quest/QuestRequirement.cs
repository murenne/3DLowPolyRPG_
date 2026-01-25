using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private Text _requireItemName;
    private Text _progressNumber;

    void Awake()
    {
        _requireItemName = GetComponent<Text>();
        _progressNumber = transform.GetChild(0).GetComponent<Text>();
    }

    /// <summary>
    /// setup requirement detail
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="totalAmount"></param>
    /// <param name="currentAmount"></param>
    public void SetupRequirement(string itemName, int totalAmount, int currentAmount)
    {
        _requireItemName.text = itemName;
        _progressNumber.text = currentAmount.ToString() + " / " + totalAmount.ToString();
    }
}
