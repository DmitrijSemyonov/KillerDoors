using KillerDoors.Services.AdsSpace;
using KillerDoors.Services.Localization;
using KillerDoors.Services.ScoreSpace;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.StateMachine;
using KillerDoors.StaticDataSpace;
using KillerDoors.UI.Common.ChangeGameState;
using KillerDoors.UI.Windows.Ads.Views;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KillerDoors.UI.Windows
{
    public class ResultCloseDoorGameWindow : Window
    {
        [field: SerializeField] public ChangeGameStateButton ÑontinueButton { get; private set; }

        [SerializeField] private TextMeshProUGUI _countCoitEarnerText;
        [SerializeField] private LocalizedText _multiplierAdsRewardText;
        [SerializeField] private AdsRequestButton _adsMultiplyCoinsButton;
        private IScoreService _scoreService;
        private GameStaticData.Ads _adsData;
        public void Construct(IAdsRewardService adsRewardService, IScoreService scoreService, IGameStateMachine gameStateMachine, IStaticDataService staticDataService)
        {
            _scoreService = scoreService;

            _adsMultiplyCoinsButton.Construct(adsRewardService);
            ÑontinueButton.Construct(gameStateMachine);
            _adsData = staticDataService.GetGameData().ads;
        }
        public void Init()
        {
            _countCoitEarnerText.text = _scoreService.EarnedCoins().ToString();
            _multiplierAdsRewardText.Localize("Multiply result", subKeys: new List<string>() { _adsData.multiplierAdsReward.ToString() });
        }

    }
}