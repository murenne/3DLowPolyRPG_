using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class ScenesManager : Singleton<ScenesManager>, IEndGameObserver
{
    [SerializeField] private SceneFade _sceneFadePrefab;
    bool _isFadeFinish;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        GameManager.Instance.AddObserver(this);
        _isFadeFinish = true;
    }

    /// <summary>
    /// transite to destination
    /// </summary>
    /// <param name="transitionController"></param>
    public void TransitionToDestination(TransitionManager transitionController)
    {
        switch (transitionController.transitionType)
        {
            case TransitionType.DifferentScene:
                GameManager.Instance.PlayerStatus.IsTransing = true;
                StartCoroutine(Transition(transitionController.sceneName, transitionController.targetTag));
                break;
            case TransitionType.SameScene:
                GameManager.Instance.PlayerStatus.IsTransing = true;
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionController.targetTag));
                break;
        }
    }

    /// <summary>
    /// execute transition
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="destinationTag"></param>
    /// <returns></returns>
    IEnumerator Transition(string sceneName, DestinationTag destinationTag)
    {
        SceneFade fade = Instantiate(_sceneFadePrefab);
        yield return StartCoroutine(fade.FadeOut(2f));
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance.SaveAllInventoryData();

        // diffience scene
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);

            yield return new WaitUntil(() =>
                GameManager.Instance != null &&
                GameManager.Instance.PlayerStatus != null &&
                GameManager.Instance.PlayerStatus.gameObject != null
            );

            yield return null;

            SaveManager.Instance.LoadPlayerData(true);

            // use portal position
            var player = GameManager.Instance.PlayerStatus.gameObject;
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            var destination = GetDestination(destinationTag);
            if (destination != null)
            {
                player.transform.SetPositionAndRotation(
                    destination.spawnTransform.position,
                    destination.spawnTransform.rotation
                );
            }

            if (cc != null) cc.enabled = true;

            yield return StartCoroutine(fade.FadeIn(2f));
            Destroy(fade.transform.gameObject);
            GameManager.Instance.PlayerStatus.IsTransing = false;
            yield break;
        }
        // same scene
        else
        {
            var player = GameManager.Instance.PlayerStatus.gameObject;
            CharacterController characterController = player.GetComponent<CharacterController>();
            characterController.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).spawnTransform.position, GetDestination(destinationTag).transform.GetChild(0).rotation);
            characterController.enabled = true;

            GameManager.Instance.PlayerStatus.IsTransing = false;
            yield return StartCoroutine(fade.FadeIn(2f));
            yield return null;
        }
    }

    /// <summary>
    /// get destination
    /// </summary>
    /// <param name="destinationTag"></param>
    /// <returns></returns>
    public TransitionManager GetDestination(DestinationTag targetTag)
    {
        var entrances = FindObjectsOfType<TransitionManager>();

        for (int i = 0; i <= entrances.Length; i++)
        {
            if (entrances[i].myTag == targetTag)
            {
                return entrances[i];
            }
        }

        return null;
    }

    /// <summary>
    /// from in-game return to main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadMain());
    }

    /// <summary>
    /// from main menu to start a new game
    /// </summary>
    public void StartNewGame()
    {
        StartCoroutine(LoadLevel("Customization"));
    }

    /// <summary>
    /// from main menu return to saved game
    /// </summary>
    public void LoadSavedGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName, true));
    }

    /// <summary>
    /// load the specified scene
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    IEnumerator LoadLevel(string scene, bool isLoadingSave = false)
    {
        SceneFade fade = Instantiate(_sceneFadePrefab);

        if (scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(2f));
            yield return SceneManager.LoadSceneAsync(scene);

            if (isLoadingSave)
            {
                yield return null;
                SaveManager.Instance.LoadPlayerData();
                InventoryManager.Instance.LoadAllInventoryData();
                InventoryManager.Instance.gameObject.SetActive(true);
            }

            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;
        }
    }

    /// <summary>
    /// return to main menu
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadMain()
    {
        SceneFade fade = Instantiate(_sceneFadePrefab);
        yield return StartCoroutine(fade.FadeOut(2f));
        InventoryManager.Instance.gameObject.SetActive(false);
        yield return SceneManager.LoadSceneAsync("MainMenu");
        yield return StartCoroutine(fade.FadeIn(2f));
        yield break;
    }

    /// <summary>
    /// player lose this game and notify end game to enemies
    /// </summary>
    public void EndGameNotify()
    {
        if (_isFadeFinish)
        {
            _isFadeFinish = false;
            StartCoroutine(LoadMain());
        }
    }
}
