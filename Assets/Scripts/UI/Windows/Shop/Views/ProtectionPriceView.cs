using Helpers;
using KillerDoors.Data.Management;
using KillerDoors.Services.ShopSpace;
using TMPro;
using UnityEngine;

namespace KillerDoors.UI.Windows.Shop.Views
{
    public class ProtectionPriceView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textNextProtectionPrice;
        [SerializeField] private AppearingAndDisappearingObject _appearingAnimation;
        [SerializeField] private BuyShopItemButton _buyProtectionButton;

        private ObservablePlayerProgressWithoutInventoryCoins _progress;

        public void Construct(ObservablePlayerProgressWithoutInventoryCoins progress, IShopService shopService)
        {
            _progress = progress;
            _buyProtectionButton.Construct(shopService);
        }
        public void Subscribes()
        {
            _progress.protectionPrice.Changed += UpdateNextProtectionPrice;
            UpdateNextProtectionPrice(_progress.protectionPrice.Value);
        }
        public void Describes()
        {
            if (_progress != null)
                _progress.protectionPrice.Changed -= UpdateNextProtectionPrice;
        }
        private void UpdateNextProtectionPrice(int price) => 
            _textNextProtectionPrice.text = price.ToString();
        private void OnDestroy() =>
            Describes();
    }
}