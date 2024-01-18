using Helpers;
using KillerDoors.Data.Management;
using TMPro;
using UnityEngine;

namespace KillerDoors.UI.Windows.Main.Views
{
    public class ProtectionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _protectionNumberText;
        [SerializeField] private AppearingAndDisappearingObject _appearingAnimation;

        private ObservablePlayerProgressWithoutInventoryCoins _progress;

        public void Construct(ObservablePlayerProgressWithoutInventoryCoins progress)
        {
            _progress = progress;
        }
        public void Subscribes()
        {
            _progress.protection.Changed += UpdateProtectionNumberText;
            UpdateProtectionNumberText(_progress.protection.Value);
        }
        public void Describes()
        {
            if (_progress != null)
                _progress.protection.Changed -= UpdateProtectionNumberText;
        }
        private void UpdateProtectionNumberText(int protection)
        {
            _protectionNumberText.text = protection.ToString();
            _appearingAnimation.StartAppearing();
        }

        private void OnDestroy() =>
            Describes();
    }
}