using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueData_SO _dialogueData;
    [SerializeField] private bool canTalk = false;

    void OnEnable()
    {
        MouseManager.Instance.onMouseClicked += HandleOpenDialogueUI;
    }

    void OnDisable()
    {
        if (!MouseManager.IsInitialized)
        {
            return;
        }

        MouseManager.Instance.onMouseClicked -= HandleOpenDialogueUI;
    }

    /// <summary>
    /// event handle : open dialog UI
    /// </summary>
    private void HandleOpenDialogueUI(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<DialogueTrigger>(out DialogueTrigger dialogueTrigger))
        {
            if (canTalk)
            {
                // setup dialogue data and display first text
                GameManager.Instance.PlayerStatus.IsTalking = true;
                DialogueUI.Instance.SetupDialogueData(_dialogueData);
                DialogueUI.Instance.UpdateMainDialogue(_dialogueData.dialoguePieces[0]);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _dialogueData != null)
        {
            canTalk = true;
        }
    }
}
