using KillerDoors.Services.AdsSpace;
using KillerDoors.Services.AssetManagement;
using KillerDoors.Services.MonoBehaviourFunctions;
using UnityEngine.Audio;

namespace KillerDoors.Services.GameSettings
{
    public class SoundService : ISoundService
    {
        private const string AudioMixerPath = "AudioMixer";
        private const string Volume = "Volume";

        private AudioMixer _audioMixer;
        private readonly IMonoBehaviourService _monoBehaviourService;
        private readonly IGameSettingsService _settingsService;
        private readonly IAdsService _adsService;

        private float _cachedVolumeSound;

        public SoundService(IAssetProvider assetProvider, IMonoBehaviourService monoBehaviourService, IGameSettingsService settingsService, IAdsService adsService)
        {
            _audioMixer = assetProvider.Load<AudioMixer>(AudioMixerPath);
            _monoBehaviourService = monoBehaviourService;
            _settingsService = settingsService;
            _adsService = adsService;

            _monoBehaviourService.ApplicationFocus += OnApplicationFocus;

            _adsService.OnAdsStarted += TempDisableSound;
            _adsService.OnAdsClosed += RecoverSound;
            _adsService.OnAdsError += RecoverSound;
        }

        public void Init()
        {
            bool isEnableSound = _settingsService.SettingsData.isSoundEnabled;
            TurnTheAudioSwitch(isEnableSound);
        }
        public void TurnTheAudioSwitch(bool value)
        {
            _cachedVolumeSound = value ? 0f : -80f;

            _audioMixer.SetFloat(Volume, _cachedVolumeSound);

            _settingsService.SettingsData.isSoundEnabled = value;
            _settingsService.Changed();
        }
        private void TempDisableSound()
        {
            _audioMixer.SetFloat(Volume, -80f);
        }
        private void RecoverSound()
        {
            _audioMixer.SetFloat(Volume, _cachedVolumeSound);
        }
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
                RecoverSound();
            else
                TempDisableSound();
        }
    }
}