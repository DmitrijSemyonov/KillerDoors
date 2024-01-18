using Helpers;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.ScoreSpace;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.Services.TimeSpace;
using KillerDoors.StaticDataSpace;
using KillerDoors.UI.Windows;
using System;

namespace KillerDoors.Services.AdsSpace
{
    public class AdsRewardService : IAdsRewardService
    {
        private readonly IAdsService _ads;
        private readonly IProgressService _progressService;
        private readonly IScoreService _scoreService;
        private readonly ITimeService _timeService;
        private readonly IStaticDataService _staticDataService;
        private IWindowsService _windowsService;

        private GameStaticData.Ads _adsData;
        public AdsRewardService(IAdsService adsService, IProgressService progressService, IScoreService scoreService, IStaticDataService staticDataService,
            ITimeService timeService)
        {
            _ads = adsService;
            _progressService = progressService;
            _scoreService = scoreService;
            _timeService = timeService;
            _staticDataService = staticDataService;
            _adsData = _staticDataService.GetGameData().ads;
        }

        public void Construct2(IWindowsService windowsService)
        {
            _windowsService = windowsService;
        }
        public void AdsRequest(AdsId forItem)
        {
            Action RewardAction = null;
            switch (forItem)
            {
                case AdsId.None:
                    break;
                case AdsId.SlowdownTime:
                    RewardAction = AdsRewardSlowdownTime;
                    break;
                case AdsId.Protection:
                    RewardAction = AdsRewardProtection;
                    break;
                case AdsId.Dinamit:
                    RewardAction = AdsRewardDinamit;
                    break;
                case AdsId.MultiplyCoins:
                    RewardAction = AdsRewardMultiplyCoins;
                    break;
            }
            _ads.ShowRewardedAds(RewardAction);
        }
        private void AdsRewardSlowdownTime()
        {
            _windowsService.AdsWindowOnScene.ButtonSlowdownTime.GetComponent<AppearingAndDisappearingObject>().StartDisappearing();
            _timeService.SetTimeScale(_adsData.timeValueAdsReward);
        }
        private void AdsRewardMultiplyCoins() =>
            _scoreService.SetMultiplyReward(_adsData.multiplierAdsReward);
        private void AdsRewardProtection() =>
            _progressService.ObservableProgress.protection.Value += _adsData.protectionCountAdsReward;
        private void AdsRewardDinamit() =>
            _progressService.ObservableProgress.dinamit.Value += _adsData.dinamitCountAdsReward;
    }
}