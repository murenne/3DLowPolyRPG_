using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    [Header("Transition Infomation")]
    public string sceneName;
    public TransitionType transitionType;
    public DestinationTag myTag;
    public DestinationTag targetTag;
    public Transform spawnTransform;

    private bool canTrans;

    void OnEnable()
    {
        MouseManager.Instance.onMouseClicked += HandleExecuteTrans;
    }

    void OnDisable()
    {
        if (!MouseManager.IsInitialized)
        {
            return;
        }

        MouseManager.Instance.onMouseClicked -= HandleExecuteTrans;
    }

    void HandleExecuteTrans(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<TransitionManager>(out TransitionManager transitionController))
        {
            if (canTrans)
            {
                GameManager.Instance.PlayerStatus.IsTransing = true;
                ScenesManager.Instance.TransitionToDestination(this);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrans = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrans = false;
        }
    }
}
