using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundView : MonoBehaviour
{
    private Toggle _soundToggle;
    private Settings _settings;
    void Awake()
    {
        _soundToggle = GameObject.Find("SoundToggle").GetComponent<Toggle>();
        _settings = FindObjectOfType<Settings>();

        _settings.SoundSwitchedAtStart += SwitchSound;

        _soundToggle.onValueChanged.AddListener(_settings.TurnTheAudioSwitch);
    }

    private void SwitchSound(bool isActive)
    {
        _soundToggle.isOn = isActive;
    }

    private void OnDestroy()
    {
        if(_settings)
           _settings.SoundSwitchedAtStart -= SwitchSound;
    }
}
