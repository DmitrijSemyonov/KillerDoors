using KillerDoors.Services.StaticDataSpace;
using KillerDoors.Services.WebMediatorSpace;
using System;

namespace KillerDoors.Services.AdsSpace
{
    public class Ads : IAdsService
    {
        public event Action OnAdsClosed;
        public event Action OnAdsStarted;
        public event Action OnAdsRewarded;
        public event Action OnAdsError;

        private readonly IWebMediatorService _webMediatorService;
        private readonly IStaticDataService _staticDataService;
        private Action OnReward;

        private bool _adsEnabled;
        public Ads(IWebMediatorService yandex, IStaticDataService staticDataService)
        {
            _webMediatorService = yandex;
            _staticDataService = staticDataService;
            _adsEnabled = _staticDataService.GetGameData().ads.AdsEnabled;

#if UNITY_WEBGL && !UNITY_EDITOR
        _webMediatorService.AdsReceivedReward += OnAdsReceivedReward;
        _webMediatorService.RewardedAdsEror += OnRewardedAdsEror;
        _webMediatorService.InterstitialAdsClosed += OnInterstitialAdsClosed;
        _webMediatorService.RewardedAdsClosed += OnRewardedAdsClosed;
#endif
        }
        public void ShowAds()
        {
            if (!_adsEnabled)
            {
                OnAdsStarted?.Invoke();
                OnAdsClosed?.Invoke();
                return;
            }

#if UNITY_WEBGL && !UNITY_EDITOR
        if (_webMediatorService != null)
            _webMediatorService.ShowInterstitialAds();
#endif
            OnAdsStarted?.Invoke();
        }
        public void ShowRewardedAds(Action OnReward)
        {
            if (!_adsEnabled)
            {
                OnAdsStarted?.Invoke();
                OnAdsRewarded?.Invoke();
                OnAdsClosed?.Invoke();
                OnReward?.Invoke();
                return;
            }

            this.OnReward = OnReward;

#if UNITY_WEBGL && !UNITY_EDITOR
        if (_webMediatorService != null)
            _webMediatorService.ShowRewardedAds();
#endif
            OnAdsStarted?.Invoke();
        }
        public void OnAdsReceivedReward()
        {
            OnAdsRewarded?.Invoke();
            OnReward?.Invoke();
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
}