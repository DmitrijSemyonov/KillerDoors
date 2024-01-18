using Helpers;
using KillerDoors.Data.Management;
using System;
using TMPro;
using UnityEngine;

namespace KillerDoors.UI.Windows.Main.Views
{
    public class PointsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _pointsCountText;
        [SerializeField] private AppearingAndDisappearingObject _pointsCountAnimation;
        private ObservablePlayerProgressWithoutInventoryCoins _progress;

        public void Construct(ObservablePlayerProgressWithoutInventoryCoins progress)
        {
            _progress = progress;
        }
        public void Subscribes()
        {
            _progress.points.Changed += UpdatePointsText;
        }
        public void Describes()
        {
            if (_progress != null)
                _progress.points.Changed -= UpdatePointsText;
        }
        private void UpdatePointsText(int coins)
        {
            _pointsCountText.text = coins > 0 ? coins.ToString() : "";
            _pointsCountAnimation.StartAppearing();
        }
        private void OnDestroy() =>
            Describes();
    }
}