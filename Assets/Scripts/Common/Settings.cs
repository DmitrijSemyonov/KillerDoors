using System;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    private const string SOUND = "Sound";
    private const string VOLUME = "Volume";
    private float _cachedVolumeSound;
    private TimeController _timeController;
    public event Action<bool> SoundSwitchedAtStart;
    void Start()
    {
        if (!PlayerPrefs.HasKey(SOUND))
        {
            PlayerPrefs.SetString(SOUND, bool.TrueString);
        }

        bool isEnableSound = bool.Parse(PlayerPrefs.GetString(SOUND));

        TurnTheAudioSwitch(isEnableSound);
        SoundSwitchedAtStart?.Invoke(isEnableSound);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Ads.instance.OnAdsStarted += TempDisableSound;
        Ads.instance.OnAdsClosed += RecoverSound;

        _timeController = FindObjectOfType<TimeController>();
        _timeController.FocusPause += TempDisableSound;
        _timeController.FocusResume += RecoverSound;
    }
    public void TurnTheAudioSwitch(bool value)
    {
        if (value)
        {
            _cachedVolumeSound = 0f;
        }
        else
        {
            _cachedVolumeSound = -80f;
        }
        _audioMixer.SetFloat(VOLUME, _cachedVolumeSound);

        PlayerPrefs.SetString(SOUND, value.ToString());
    }
    private void TempDisableSound()
    {
        _audioMixer.SetFloat(VOLUME, -80f);
    }
    private void RecoverSound()
    {
        _audioMixer.SetFloat(VOLUME, _cachedVolumeSound);
    }
    public void AuthorizationInYandex()
    {
        if (Yandex.instance != null)
        {
            Yandex.instance.OpenAuthDialog();
        }
    }
    private void OnDestroy()
    {
        if(Ads.instance != null)
        {
            Ads.instance.OnAdsStarted -= TempDisableSound;
            Ads.instance.OnAdsClosed -= RecoverSound;
        }
        if (_timeController)
        {
            _timeController.FocusPause -= TempDisableSound;
            _timeController.FocusResume -= RecoverSound;
        }
    }
}
