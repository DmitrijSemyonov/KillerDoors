using Helpers;
using KillerDoors.Data.Management;
using KillerDoors.UI.Common.ChangeGameState;
using KillerDoors.UI.StaticDataSpace;
using KillerDoors.UI.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillerDoors.UI.Common
{
    public class ScoreCoinsView : MonoBehaviour
    {
        [field: SerializeField] public Button SwitchShopButton { get; private set; }

        [SerializeField] private TextMeshProUGUI _coinsCountText;
        [SerializeField] private AppearingAndDisappearingObject _coinsCountAnimation;
        [SerializeField] private Animator _coinsAnimator;

        private int _fastRotateHash = Animator.StringToHash("fastRotate");

        private ObservablePlayerProgressWithoutInventoryCoins _progress;
        private IWindowsService _windowService;

        private ChangeGameStateButton _playCloseDoorsModeButton;

        public void Construct(ObservablePlayerProgressWithoutInventoryCoins progress, IWindowsService windowService, ChangeGameStateButton playCloseDoorsModeButton)
        {
            _progress = progress;
            _windowService = windowService;
            SwitchShopButton.onClick.AddListener(SwitchShop);
            _playCloseDoorsModeButton = playCloseDoorsModeButton;
        }
        public void Subscribes()
        {
            _progress.scoreCoins.Changed += UpdateCoinsText;
            UpdateCoinsText(_progress.scoreCoins.Value);
        }
        public void Describes()
        {
            if (_progress != null)
                _progress.scoreCoins.Changed -= UpdateCoinsText;
        }
        private void SwitchShop()
        {
            if (_windowService.ShopWindowOnScene)
            {
                _windowService.Close(WindowId.Shop);

                _playCloseDoorsModeButton.ShowButton();

                _windowService.Open(WindowId.Ads);

                if (Time.timeScale < 1)
                    _windowService.AdsWindowOnScene.ButtonSlowdownTime.GetComponent<AppearingAndDisappearingObject>().StartDisappearing();
            }
            else
            {
                _windowService.Open(WindowId.Shop);

                _playCloseDoorsModeButton.HideButton();

                _windowService.Close(WindowId.Ads);
            }
        }
        private void UpdateCoinsText(int coins)
        {
            _coinsCountText.text = coins.ToString();
            _coinsCountAnimation.StartAppearing();
            _coinsAnimator.SetTrigger(_fastRotateHash);
        }
        private void OnDestroy() => 
            Describes();
    }
}