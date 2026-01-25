using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button _newGameButton;
    Button _continueGameButton;
    Button _exitButton;
    PlayableDirector _playableDirector;

    void Awake()
    {
        _newGameButton = transform.GetChild(1).GetComponent<Button>();
        _continueGameButton = transform.GetChild(2).GetComponent<Button>();
        _exitButton = transform.GetChild(3).GetComponent<Button>();

        _newGameButton.onClick.AddListener(PlayTimeline);
        _continueGameButton.onClick.AddListener(ContinueGame);
        _exitButton.onClick.AddListener(ExitGame);

        _playableDirector = FindObjectOfType<PlayableDirector>();
        _playableDirector.stopped += NewGame;
    }

    /// <summary>
    /// start play camera timeline
    /// </summary>
    void PlayTimeline()
    {
        _playableDirector.Play();
    }

    /// <summary>
    /// start a new game
    /// </summary>
    /// <param name="_playableDirector"></param>
    void NewGame(PlayableDirector _playableDirector)
    {
        // delete all save data
        PlayerPrefs.DeleteAll();
        ScenesManager.Instance.StartNewGame();
    }

    /// <summary>
    /// load saved game
    /// </summary>
    void ContinueGame()
    {
        ScenesManager.Instance.LoadSavedGame();
    }

    /// <summary>
    /// exit game
    /// </summary>
    void ExitGame()
    {
        Application.Quit();
    }
}
