using UnityEngine;
using System.Collections.Generic;
using KillerDoors.Data.Management;
using KillerDoors.Services.AssetManagement;
using KillerDoors.Services.UpgradeDoor;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.UI.Windows.Shop.Views;

namespace KillerDoors.UI.Windows.Shop
{
    public class UpgradeDoorViewsContainer : MonoBehaviour
    {
        private IAssetProvider _assetProvider;
        private ObservablePlayerProgressWithoutInventoryCoins _progress;
        private IDoorUpgradeService _doorUpgradeService;
        private IStaticDataService _staticDataService;
        private List<UpgradeDoorView> _upgradeDoorViews = new List<UpgradeDoorView>();
        private const string UpgradeDoorViewPath = "UI/UpgradeDoorView";

        public void Construct(IAssetProvider assetProvider, ObservablePlayerProgressWithoutInventoryCoins progress, IDoorUpgradeService doorUpgradeService,
             IStaticDataService staticDataService)
        {
            _assetProvider = assetProvider;
            _progress = progress;
            _doorUpgradeService = doorUpgradeService;
            _staticDataService = staticDataService;
        }
        public void Init()
        {
            ClearViews();
            CreateUpgradeViews();
        }

        private void CreateUpgradeViews()
        {
            for (int i = 0; i < _doorUpgradeService.Doors.Count; i++)
            {
                UpgradeDoorView view = _assetProvider.Instantiate<UpgradeDoorView>(UpgradeDoorViewPath, transform);
                view.Construct(_progress, _doorUpgradeService, _staticDataService, i, _doorUpgradeService.Doors[i]);
                view.Subscribes();
                _upgradeDoorViews.Add(view);
            }
        }
        private void ClearViews()
        {
            foreach (UpgradeDoorView view in _upgradeDoorViews)
            {
                Destroy(view.gameObject);
            }
            _upgradeDoorViews.Clear();
        }
    }
}