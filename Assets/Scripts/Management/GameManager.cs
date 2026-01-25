using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField] public UnitStatus PlayerStatus { get; set; }
    private CinemachineFreeLook _followCamera;
    private List<IEndGameObserver> _endGameObserversList = new();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void RigisterPlayer(UnitStatus player)
    {
        PlayerStatus = player;
        _followCamera = FindObjectOfType<CinemachineFreeLook>();

        if (_followCamera != null)
        {
            _followCamera.Follow = PlayerStatus.transform.GetChild(0);
            _followCamera.LookAt = PlayerStatus.transform.GetChild(0);
        }
    }

    /// <summary>
    /// add observer to the list
    /// </summary>
    /// <param name="observer"></param>
    public void AddObserver(IEndGameObserver observer)
    {
        _endGameObserversList.Add(observer);
    }

    /// <summary>
    /// remove observer from list
    /// </summary>
    /// <param name="observer"></param>
    public void RemoveObserver(IEndGameObserver observer)
    {
        _endGameObserversList.Remove(observer);
    }

    /// <summary>
    /// notify observers that this game is over
    /// </summary>
    public void NotifyObservers()
    {
        foreach (var observer in _endGameObserversList)
        {
            observer.EndGameNotify();
        }
    }

    /// <summary>
    /// transition position
    /// </summary>
    /// <returns></returns>
    public Transform GetEntrance()
    {
        foreach (var transitionController in FindObjectsOfType<TransitionManager>())
        {
            if (transitionController.targetTag == DestinationTag.C)
            {
                return transitionController.spawnTransform;
            }
        }
        return null;
    }
}
