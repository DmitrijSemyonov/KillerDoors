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

        _settings.SoundSwitchedAtStart += (b) => { _soundToggle.isOn = b; };

        _soundToggle.onValueChanged.AddListener(_settings.TurnTheAudioSwitch);
    }
}
