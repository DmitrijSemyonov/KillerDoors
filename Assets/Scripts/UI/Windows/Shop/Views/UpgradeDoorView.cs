using Helpers;
using KillerDoors.Common;
using KillerDoors.Data.Management;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.Services.UpgradeDoor;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KillerDoors.UI.Windows.Shop.Views
{
    public class UpgradeDoorView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nextUpgradePriceText;
        [SerializeField] private AppearingAndDisappearingObject _nextUpgradePriceAppearing;
        [SerializeField] private Button _button;

        private IDoorUpgradeService _doorUpgrade;
        private IStaticDataService _staticDataService;
        private ObservablePlayerProgressWithoutInventoryCoins _progress;

        private Transform _door;
        private Vector3 _offsetPosition;
        private int _doorIndex;
        private Camera _mainCamera;

        public void Construct(ObservablePlayerProgressWithoutInventoryCoins progress, IDoorUpgradeService doorUpgrade,
            IStaticDataService staticDataService, int indexDoor, Door door)
        {
            _doorUpgrade = doorUpgrade;
            _staticDataService = staticDataService;
            _progress = progress;
            _door = door.transform;
            _doorIndex = indexDoor;

            _offsetPosition = _staticDataService.ForLevel(SceneManager.GetActiveScene().name).doorsDatas[_doorIndex].doorUpgradeViewOffset;

            _button.onClick.AddListener(UpgradeDoor);

            _mainCamera = Camera.main;
        }
        public void Subscribes()
        {
            _progress.upgradeDoorsPrices[_doorIndex].Changed += UpdateNextUpgradePrice;
            UpdateNextUpgradePrice(_progress.upgradeDoorsPrices[_doorIndex].Value);
        }
        public void Describes()
        {
            if (_progress != null)
                _progress.upgradeDoorsPrices[_doorIndex].Changed -= UpdateNextUpgradePrice;
        }
        public void UpgradeDoor() =>
            _doorUpgrade.UpgradeDoor(_doorIndex);
        private void Update()
        {
            UpdateViewPosition();
        }
        private void UpdateViewPosition() =>
            ((RectTransform)transform).position = _mainCamera.WorldToScreenPoint(_door.position) + _offsetPosition;
        private void UpdateNextUpgradePrice(int price)
        {
            _nextUpgradePriceText.text = price.ToString();
            _nextUpgradePriceAppearing.StartAppearing();
        }
        private void OnDestroy() => 
            Describes();
    }
}