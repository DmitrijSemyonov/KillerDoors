using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsInappOpportunities : MonoBehaviour
{
    private LosingZone _losingZone;
    private Shop _shop;
    private TimeController _timeController;
    private ScoreAccounting _scoreAccounting;
    private Inventory _inventory;
    private UIManager _uiManager;
    private bool _slowdownTimeRequest;
    private bool _multiplyCoinsRequest;
    private bool _protectionRequest;
    private bool _dinamitRequest;

    private void Start()
    {
        _losingZone = FindObjectOfType<LosingZone>();
        _shop = GetComponent<Shop>();
        _timeController = FindObjectOfType<TimeController>();
        _scoreAccounting = GetComponent<ScoreAccounting>();
        _inventory = FindObjectOfType<Inventory>();
        _uiManager = GetComponent<UIManager>();
        Ads.instance.OnAdsRewarded += AdsReward;
        Ads.instance.OnAdsError += ResetRequests;
    }
    public void SlowdownTimeRequest()
    {
        _slowdownTimeRequest = true;
        Ads.instance.ShowRewardedAds();
    }
    public void MultiplyCoinsRequest()
    {
        _multiplyCoinsRequest = true;
        Ads.instance.ShowRewardedAds();
    }
    public void ProtectionRequest()
    {
        _protectionRequest = true;
        Ads.instance.ShowRewardedAds();
    }
    public void DinamitRequest()
    {
        _dinamitRequest = true; 
        Ads.instance.ShowRewardedAds();
    }
    private void AdsReward()
    {
        if (_slowdownTimeRequest)
        {
            AdsRewardSlowdownTime();
        }
        else if (_multiplyCoinsRequest)
        {
            AdsRewardMultiplyCoins();
        }
        else if (_protectionRequest)
        {
            AdsRewardProtection();
        }
        else if (_dinamitRequest)
        {
            AdsRewardDinamit();
        }
        ResetRequests();
    }
    private void ResetRequests()
    {
        _slowdownTimeRequest = false;
        _multiplyCoinsRequest = false;
        _protectionRequest = false;
        _dinamitRequest = false;
    }

    private void AdsRewardSlowdownTime()
    {
        _uiManager.SetActiveSlowdownTimeButton(false);
        _timeController.DecreaseTimeScale();
    }
    private void AdsRewardMultiplyCoins()
    {
        _scoreAccounting.SetMultiplyReward(3);
    }
    private void AdsRewardProtection()
    {
        _losingZone.Protection += 2;
    }
    private void AdsRewardDinamit()
    {
        _shop.DinamitCount += 2;
    }
    private void OnDestroy()
    {
        if (Ads.instance != null)
        {
            Ads.instance.OnAdsRewarded -= AdsReward;
            Ads.instance.OnAdsError -= ResetRequests;
        }

    }
}
