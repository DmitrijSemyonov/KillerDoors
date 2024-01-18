using Helpers;
using KillerDoors.Data.Management;
using KillerDoors.Services.PersonSpawn;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KillerDoors.UI.Windows.Main.Views
{
    public class DinamitView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dinamitNumberText;
        [SerializeField] private AppearingAndDisappearingObject _dinamitNumberAppearing;
        [SerializeField] private Button _exploseAllButton;

        private ObservablePlayerProgressWithoutInventoryCoins _progress;
        private IPersonSpawner _personSpawner;
        public void Construct(ObservablePlayerProgressWithoutInventoryCoins progress, IPersonSpawner personSpawner)
        {
            _progress = progress;
            _personSpawner = personSpawner;
            _exploseAllButton.onClick.AddListener(ExploseAll);
        }
        public void Subscribes()
        {
            _progress.dinamit.Changed += UpdateDinamitNumberText;
            UpdateDinamitNumberText(_progress.dinamit.Value);
        }
        public void Describes()
        {
            if (_progress != null)
                _progress.dinamit.Changed -= UpdateDinamitNumberText;
        }
        private void UpdateDinamitNumberText(int protection)
        {
            _dinamitNumberText.text = protection.ToString();
            _dinamitNumberAppearing.StartAppearing();
        }
        private void ExploseAll() =>
            _personSpawner.TryExploseAll();
        private void OnDestroy() =>
            Describes();
    }
}