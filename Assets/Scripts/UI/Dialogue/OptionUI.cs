using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Text _optionText;
    private Button _thisOptionButton;
    private DialoguePiece _currentPiece;
    private string _nextPieceID;
    private bool _isTakedThisQuest;

    void Awake()
    {
        _thisOptionButton = GetComponent<Button>();
        _thisOptionButton.onClick.AddListener(OnOptionClicked);
    }

    /// <summary>
    /// setup dialog option
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="option"></param>
    public void SetupDialogueOption(DialoguePiece piece, DialogueOption option)
    {
        _currentPiece = piece;
        _optionText.text = option.text;
        _nextPieceID = option.targetID;
        _isTakedThisQuest = option.isDecideToTakeThisQuest;
    }

    /// <summary>
    /// click event
    /// </summary>
    public void OnOptionClicked()
    {
        // setup quest option
        if (_currentPiece.quest != null)
        {
            var newTask = new QuestManager.Quest
            {
                _questData = Instantiate(_currentPiece.quest)
            };

            // take quest but quest management does's have quest data
            if (_isTakedThisQuest && !QuestManager.Instance.HaveQuest(newTask._questData))
            {
                QuestManager.Instance.questList.Add(newTask);
                QuestManager.Instance.GetTask(newTask._questData).IsStarted = true;
            }
        }

        // display next dialog text
        if (_nextPieceID == "")
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            return;
        }
        else
        {
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentData.dialogueIndex[_nextPieceID]);
        }
    }
}
