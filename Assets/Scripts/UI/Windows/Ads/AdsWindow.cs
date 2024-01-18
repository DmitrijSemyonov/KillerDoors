using UnityEngine;
using KillerDoors.Services.AdsSpace;
using KillerDoors.UI.Windows.Ads.Views;

namespace KillerDoors.UI.Windows
{
    public class AdsWindow : Window
    {
        [field: SerializeField] public AdsRequestButton ButtonSlowdownTime { get; private set; }
        [SerializeField] private AdsRequestButton _buttonProtection;
        [SerializeField] private AdsRequestButton _buttonDinamit;

        public void Construct(IAdsRewardService adsRewardService)
        {
            ButtonSlowdownTime.Construct(adsRewardService);
            _buttonProtection.Construct(adsRewardService);
            _buttonDinamit.Construct(adsRewardService);
        }
    }
}