using KillerDoors.Data;
using KillerDoors.Services.Localization;
using KillerDoors.Services.SaveLoad;
using KillerDoors.UI;
using System;

namespace KillerDoors.Services.WebMediatorSpace
{
    public interface IWebMediatorService : IService
    {
        bool IsSDKInitialized { get; }
        bool IsAuthorized { get; }

        event Action Authorized;
        event Action AdsReceivedReward;
        event Action RewardedAdsClosed;
        event Action RewardedAdsEror;
        event Action InterstitialAdsClosed;

        void Construct(ISaveLoadService saveLoadService, ILocalizationService localizationService);
        void OpenAuthDialog();
        void SavePlayerData(PlayerProgress data);
        void SetLoadingPanel(LoadingPanel loadingPanel);
        void ShowInterstitialAds();
        void ShowRewardedAds();
    }
}