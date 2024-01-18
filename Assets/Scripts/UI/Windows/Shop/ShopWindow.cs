using UnityEngine;
using KillerDoors.Data.Management;
using KillerDoors.Services.AssetManagement;
using KillerDoors.Services.UpgradeDoor;
using KillerDoors.Services.ShopSpace;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.UI.Windows.Shop;
using KillerDoors.UI.Windows.Shop.Views;

namespace KillerDoors.UI.Windows
{
    public class ShopWindow : Window
    {
        [SerializeField] private DinamitPriceView _dinamitPriceView;
        [SerializeField] private ProtectionPriceView _protectionPriceView;
        [SerializeField] private UpgradeDoorViewsContainer _upgradeDoorViewsContainer;

        public void Construct(ObservablePlayerProgressWithoutInventoryCoins progress, IStaticDataService staticDataService,
            IDoorUpgradeService doorUpgradeService, IAssetProvider assetProvider, IShopService shopService)
        {
            _dinamitPriceView.Construct(progress, shopService);
            _protectionPriceView.Construct(progress, shopService);
            _upgradeDoorViewsContainer.Construct(assetProvider, progress, doorUpgradeService, staticDataService);
        }
        public void Init()
        {
            _upgradeDoorViewsContainer.Init();
        }
        public void Subscribes()
        {
            _dinamitPriceView.Subscribes();
            _protectionPriceView.Subscribes();
        }
    }
}