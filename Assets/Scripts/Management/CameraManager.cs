using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;

    void OnEnable()
    {
        if (CustomizationManager.Instance != null)
        {
            CustomizationManager.Instance.OnCharacterCreated += HandleCharacterCreated;
        }
    }

    void OnDisable()
    {
        if (CustomizationManager.Instance != null)
        {
            CustomizationManager.Instance.OnCharacterCreated -= HandleCharacterCreated;
        }
    }

    /// <summary>
    /// event: set camera follow and look at
    /// </summary>
    /// <param name="player"></param>
    void HandleCharacterCreated(GameObject player)
    {
        if (freeLookCamera != null)
        {
            freeLookCamera.Follow = player.transform.GetChild(1).transform;
            freeLookCamera.LookAt = player.transform.GetChild(1).transform;
        }
    }
}
