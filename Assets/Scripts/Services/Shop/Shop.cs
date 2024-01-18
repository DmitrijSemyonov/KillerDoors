using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.StaticDataSpace;

namespace KillerDoors.Services.ShopSpace
{
    public class Shop : IShopService
    {
        private IProgressService _progressService;
        private IStaticDataService _staticDataService;

        public Shop(IProgressService progressService, IStaticDataService staticDataService)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
        }
        public void Init()
        {
            _progressService.ObservableProgress.protection.Changed += UpdateProtectionPrice;
            _progressService.ObservableProgress.dinamit.Changed += UpdateDinamitPrice;
            UpdateProtectionPrice(_progressService.ObservableProgress.protection.Value);
            UpdateDinamitPrice(_progressService.ObservableProgress.dinamit.Value);
        }
        public void BuyItem(ShopItem item)
        {
            switch (item)
            {
                case ShopItem.Protection:
                    BuyProtection();
                    break;
                case ShopItem.Dinamit:
                    BuyDinamit();
                    break;
            }
        }
        private void BuyProtection()
        {
            if (_progressService.ObservableProgress.scoreCoins.Value < _progressService.ObservableProgress.protectionPrice.Value) return;

            _progressService.ObservableProgress.SpendScoreCoins(_progressService.ObservableProgress.protectionPrice.Value);
            _progressService.ObservableProgress.protection.Value++;
        }
        private void BuyDinamit()
        {
            if (_progressService.ObservableProgress.scoreCoins.Value < _progressService.ObservableProgress.dinamitPrice.Value) return;

            _progressService.ObservableProgress.SpendScoreCoins(_progressService.ObservableProgress.dinamitPrice.Value);
            _progressService.ObservableProgress.dinamit.Value++;
        }
        private void UpdateProtectionPrice(int protectionCount)
        {
            int[] protectionPrices = _staticDataService.GetGameData().prices.protectionPrices;
            if (protectionCount >= protectionPrices.Length)
                _progressService.ObservableProgress.protectionPrice.Value = protectionPrices[protectionPrices.Length - 1];
            else
                _progressService.ObservableProgress.protectionPrice.Value = protectionPrices[protectionCount];
        }
        private void UpdateDinamitPrice(int dinamitCount)
        {
            int[] dinamitPrices = _staticDataService.GetGameData().prices.dinamitPrices;
            if (dinamitCount >= dinamitPrices.Length)
                _progressService.ObservableProgress.dinamitPrice.Value = dinamitPrices[dinamitPrices.Length - 1];
            else
                _progressService.ObservableProgress.dinamitPrice.Value = dinamitPrices[dinamitCount];
        }
    }
}