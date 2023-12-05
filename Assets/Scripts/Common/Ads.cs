using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
//using CrazyGames;

public class Ads : MonoBehaviour
{
    public event Action OnAdsClosed;
    public event Action OnAdsStarted;
    public event Action OnAdsRewarded;
    public event Action OnAdsError;
    [SerializeField] private bool _isAdsEnabled = true;
    public bool IsAdsEnabled => _isAdsEnabled;

    public static Ads instance;
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void ShowAds()
    {
        if (!_isAdsEnabled)
        {
            OnAdsStarted?.Invoke();
            OnAdsClosed?.Invoke();
            return;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        if (Yandex.instance != null)
        {
            Yandex.instance.ShowInterstitialAds();
        }
#endif
        OnAdsStarted?.Invoke();
    }
    public void ShowRewardedAds()
    {
        if (!_isAdsEnabled)
        {
            OnAdsStarted?.Invoke();
            OnAdsRewarded?.Invoke();
            OnAdsClosed?.Invoke();
            return;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        if (Yandex.instance != null)
        {
            Yandex.instance.ShowRewardedAds();
        }
#endif
        OnAdsStarted?.Invoke();
    }
    public void OnAdsReceivedReward()
    {
        OnAdsRewarded?.Invoke();
    }
    public void OnRewardedAdsClosed()
    {
        OnAdsClosed?.Invoke();
    }
    public void OnRewardedAdsEror()
    {
        OnAdsError?.Invoke();
    }
    public void OnInterstitialAdsClosed()
    {
        OnAdsClosed?.Invoke();
    }
}