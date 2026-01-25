using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elments")]
    public Image icon;
    public Text mainText;
    public Button nextButton;
    public GameObject dialoguePanel;

    [Header("Options")]
    public RectTransform optionPanel;
    public OptionUI optionPrefab;

    [Header("Data")]
    public DialogueData_SO currentData;
    int currentIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }
    void Update()
    {
        GameManager.Instance.PlayerStatus.IsTalking = dialoguePanel.activeInHierarchy;
    }

    /// <summary>
    /// set up dialogue data
    /// </summary>
    /// <param name="data"></param>
    public void SetupDialogueData(DialogueData_SO data)
    {
        currentData = data;
        currentIndex = 0;
    }

    /// <summary>
    /// update dialogue text and icon
    /// </summary>
    /// <param name="piece"></param>
    public void UpdateMainDialogue(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);
        currentIndex++;

        // icon
        if (piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
        {
            icon.enabled = false;
        }

        // dialogue text
        mainText.text = "";
        mainText.DOText(piece.text, 2f);

        // option 
        if (piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)
        {
            // display next button
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            // hide next button's text (next)
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
        }

        // creat options
        CreatOptions(piece);
    }

    /// <summary>
    /// creat options
    /// </summary>
    /// <param name="piece"></param>
    void CreatOptions(DialoguePiece piece)
    {
        // Initialize (delete all past options)
        if (optionPanel.childCount > 0)
        {
            for (int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }

        // read this piece's option data, and creat a new option object
        for (int i = 0; i < piece.options.Count; i++)
        {
            var option = Instantiate(optionPrefab, optionPanel);
            option.SetupDialogueOption(piece, piece.options[i]);
        }
    }

    /// <summary>
    /// next botton clicked event
    /// </summary>
    private void OnNextButtonClicked()
    {
        if (currentIndex < currentData.dialoguePieces.Count)
        {
            UpdateMainDialogue(currentData.dialoguePieces[currentIndex]);
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }
}
