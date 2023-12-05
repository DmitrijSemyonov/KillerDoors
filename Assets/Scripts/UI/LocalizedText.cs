using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private string key;
    public event Action localized;
    void Start()
    {
        if (!text)
        {
            Init();
            Localize(key);
        }
        LocalizationManager.OnLanguageChange += OnLanguageChange;
    }
    private void OnDestroy()
    {
        LocalizationManager.OnLanguageChange -= OnLanguageChange;
    }
    private void OnLanguageChange()
    {
        Localize(key);
    }

    private void Init()
    {
        text = GetComponent<TextMeshProUGUI>();
        key = text.text;
    }
    public void Localize(string newKey = null)
    {
        if(text == null)
        {
            Init();
        }
        if(newKey != null)
        {
            key = newKey;
        }
        if (key.Contains("IGNORELOCALIZE"))
        {
            return;
        }
        string value = LocalizationManager.GetTranslate(key);
        if (value.Contains("NEWLINE"))
        {
            value = value.Replace("NEWLINE", Environment.NewLine);
        }
        text.text = value;
        localized?.Invoke();
    }
}
