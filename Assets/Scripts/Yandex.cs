using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Yandex : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GiveMePlayerData();    
    [DllImport("__Internal")]
    private static extern void AuthorizationYG();

    [DllImport("__Internal")]
    private static extern void OpenAuthDialogYG();

    [DllImport("__Internal")]
    private static extern void RateGame();

    [DllImport("__Internal")]
    private static extern void SaveExtern(string data);

    [DllImport("__Internal")]
    private static extern void LoadExtern();
    [DllImport("__Internal")]
    private static extern string GetLanguage();
    [DllImport("__Internal")]
    private static extern string GetLanguageI18N();
    [DllImport("__Internal")]
    private static extern string ShowInterstitial();
    [DllImport("__Internal")]
    private static extern string ShowRewarded();

    public static Yandex instance { get; private set; }
    private GameObject _yandexAuthButton;
    private bool _isAuthorized;
    public bool IsYsdkInitialized { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _yandexAuthButton = GameObject.Find("Log&SaveInCloudButton");
        if (_isAuthorized)
        {
            _yandexAuthButton.SetActive(false);
        }
    }
    public void IdentifiedDesctop()
    {
        Application.runInBackground = false;
    }
    public void IdentifiedMobile()
    {
    }
    public void HideAuthorizationYandexButtonAndLoadData()
    {
        _yandexAuthButton = GameObject.Find("Log&SaveInCloudButton");
        if (_yandexAuthButton)
        {
            _yandexAuthButton.SetActive(false);
        }
        _isAuthorized = true;
        LoadExtern();
    }
    public void Authorization()
    {
        IsYsdkInitialized = true;
        AuthorizationYG();
    }
    public void OpenAuthDialog()
    {
        OpenAuthDialogYG();
    }
    public void ShowInterstitialAds()
    {
        ShowInterstitial();
    }
    public void ShowRewardedAds()
    {
        ShowRewarded();
    }
    public string GetBrowserLanguage()
    {
        return GetLanguage();
    }
    public string Geti18NLanguage()
    {
        return GetLanguageI18N();
    }
    public void SavePlayerData(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        SaveExtern(jsonData);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
