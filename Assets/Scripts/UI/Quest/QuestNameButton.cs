using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    [SerializeField] private Text _questNameText;
    [SerializeField] QuestData_SO _questData;
    private Text _questContentText;
    public Text QuestContentText { get => _questContentText; set => _questContentText = value; }

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    /// <summary>
    /// when you click quest name button, the quest description will update in right
    /// </summary>
    void UpdateQuestContent()
    {
        _questContentText.text = _questData.description;
        QuestUI.Instance.SetupRequireItemList(_questData);
        QuestUI.Instance.SetupRewardItemList(_questData);
    }

    /// <summary>
    /// setup quest name button's name
    /// </summary>
    /// <param name="questData"></param>
    public void SetupQuestNameButton(QuestData_SO questData)
    {
        _questData = questData;
        _questNameText.text = _questData.isCompleted ? _questData.questName + "Completed" : _questData.questName;
    }
}
