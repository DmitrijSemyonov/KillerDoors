using Helpers;
using KillerDoors.Data.Management;
using KillerDoors.Services.ShopSpace;
using TMPro;
using UnityEngine;

namespace KillerDoors.UI.Windows.Shop.Views
{
    public class DinamitPriceView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dinamitNextPriceText;
        [SerializeField] private AppearingAndDisappearingObject _dinamitNextPriceAppearing;
        [SerializeField] private BuyShopItemButton _buyDinamitButton;

        private ObservablePlayerProgressWithoutInventoryCoins _progress;

        public void Construct(ObservablePlayerProgressWithoutInventoryCoins progress, IShopService shopService)
        {
            _progress = progress;
            _buyDinamitButton.Construct(shopService);
        }
        public void Subscribes()
        {
            _progress.dinamitPrice.Changed += UpdateNextDinamitPriceText;
            UpdateNextDinamitPriceText(_progress.dinamitPrice.Value);
        }
        public void Describes()
        {
            if (_progress != null)
                _progress.dinamitPrice.Changed -= UpdateNextDinamitPriceText;
        }
        private void UpdateNextDinamitPriceText(int price)
        {
            _dinamitNextPriceText.text = price.ToString();
            _dinamitNextPriceAppearing.StartAppearing();
        }
        private void OnDestroy() =>
            Describes();
    }
}