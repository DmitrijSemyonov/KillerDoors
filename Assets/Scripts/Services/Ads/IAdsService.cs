using System;

namespace KillerDoors.Services.AdsSpace
{
    public interface IAdsService : IService
    {
        event Action OnAdsClosed;
        event Action OnAdsStarted;
        event Action OnAdsRewarded;
        event Action OnAdsError;

        void OnAdsReceivedReward();
        void OnInterstitialAdsClosed();
        void OnRewardedAdsClosed();
        void OnRewardedAdsEror();
        void ShowAds();
        void ShowRewardedAds(Action OnReward);
    }
}