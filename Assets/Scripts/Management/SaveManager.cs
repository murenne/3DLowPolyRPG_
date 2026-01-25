using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    private string _sceneName = "sceneName";
    public string SceneName => PlayerPrefs.GetString(_sceneName);

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScenesManager.Instance.ReturnToMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SavePlayerData();
            InventoryManager.Instance.SaveAllInventoryData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ScenesManager.Instance.LoadSavedGame();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            ClearAllSaves();
        }
    }

    /// <summary>
    /// save player data
    /// </summary>
    public void SavePlayerData()
    {
        Debug.Log("you have saved data");
        Save(GameManager.Instance.PlayerStatus.RuntimeStatusData.name, GameManager.Instance.PlayerStatus.RuntimeStatusData);

        var player = GameManager.Instance.PlayerStatus.gameObject;
        Vector3 pos = player.transform.position;
        PlayerPrefs.SetFloat("PlayerPosX", pos.x);
        PlayerPrefs.SetFloat("PlayerPosY", pos.y);
        PlayerPrefs.SetFloat("PlayerPosZ", pos.z);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// load player data
    /// </summary>
    public void LoadPlayerData(bool usePortalPosition = false)
    {
        Debug.Log("you have loaded data");
        Load(GameManager.Instance.PlayerStatus.RuntimeStatusData.name, GameManager.Instance.PlayerStatus.RuntimeStatusData);

        if (!usePortalPosition)
        {
            if (PlayerPrefs.HasKey("PlayerPosX"))
            {
                var player = GameManager.Instance.PlayerStatus.gameObject;
                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                Vector3 savedPos = new Vector3(
                    PlayerPrefs.GetFloat("PlayerPosX"),
                    PlayerPrefs.GetFloat("PlayerPosY"),
                    PlayerPrefs.GetFloat("PlayerPosZ")
                );
                player.transform.position = savedPos;

                if (cc != null) cc.enabled = true;
            }
        }

        GameManager.Instance.PlayerStatus.ApplyLoadedData();
    }

    /// <summary>
    /// use json to save data 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    public void Save(string key, object data)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(_sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// load data from json
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    public void Load(string key, object data)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }

    /// <summary>
    /// delete all save data
    /// </summary>
    public void ClearAllSaves()
    {
        Debug.Log("you have delete all data");
        PlayerPrefs.DeleteAll();
    }
}
