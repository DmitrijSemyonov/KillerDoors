using KillerDoors.Services.AdsSpace;
using UnityEngine;
using UnityEngine.UI;

namespace KillerDoors.UI.Windows.Ads.Views
{
    public class AdsRequestButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private AdsId _item;
        private IAdsRewardService _adsRewardService;

        public void Construct(IAdsRewardService adsRewardService)
        {
            _adsRewardService = adsRewardService;
            _button.onClick.AddListener(BuyItem);
        }

        public void BuyItem() => 
            _adsRewardService.AdsRequest(_item);
    }
}