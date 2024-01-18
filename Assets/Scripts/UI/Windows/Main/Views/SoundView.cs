using KillerDoors.Services.GameSettings;
using UnityEngine;
using UnityEngine.UI;

namespace KillerDoors.UI.Windows.Main.Views
{
    public class SoundView : MonoBehaviour
    {
        [field: SerializeField]
        public Toggle SoundToggle { get; private set; }
        private ISoundService _soundService;

        public void Construct(ISoundService soundService, IGameSettingsService gameSettingsService)
        {
            _soundService = soundService;
            SoundToggle.isOn = gameSettingsService.SettingsData.isSoundEnabled;
            SoundToggle.onValueChanged.AddListener(_soundService.TurnTheAudioSwitch);
        }
        private void OnDestroy()
        {
            if (_soundService != null)
                SoundToggle.onValueChanged.RemoveListener(_soundService.TurnTheAudioSwitch);
        }
    }
}