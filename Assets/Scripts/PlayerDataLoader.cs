using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataLoader : MonoBehaviour
{
    public event Action OnUpdate;
    public PlayerData playerData;
    public const string PLAYERDATA = "PlayerData";

    public static PlayerDataLoader Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (!PlayerPrefs.HasKey(PLAYERDATA))
        {
            PlayerData data = new PlayerData();
            PlayerPrefs.SetString(PLAYERDATA, JsonUtility.ToJson(data));
        }
        playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(PLAYERDATA));
    }
    public void CompareDataFromTheDeviceAndFromTheCloud(string jsonDataFromTheCloud)
    {
        PlayerData data = JsonUtility.FromJson<PlayerData>(jsonDataFromTheCloud);
        if (data.scoreCoins >= playerData.scoreCoins)
        {
            LoadData(data);
            SaveOnDevice();
        }
        else
        {
            SaveData();
        }
    }
    public void LoadData(PlayerData data)
    {
        playerData = data;
        OnUpdate?.Invoke();
    }
    public void SaveData()
    {
        if (playerData == null)
        {
            Debug.Log("Nothing to save");
            return;
        }
        SaveOnDevice();
#if UNITY_WEBGL && !UNITY_EDITOR
        if (Yandex.instance != null && Yandex.instance.IsYsdkInitialized)
        {
            Yandex.instance.SavePlayerData(playerData);
        }
#endif
    }
    private void SaveOnDevice()
    {
        PlayerPrefs.SetString(PLAYERDATA, JsonUtility.ToJson(playerData));
    }
}
[System.Serializable]
public class PlayerData
{
    public List<int> upgradesCoins = new List<int>();
    public int scoreCoins;
    public int protection;
    public int dinamit;
    public List<int> upgradeCountsDoors = new List<int>() {1,1,1,1,1,1,1 };
    public bool educationCompleted;
}
