using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static int SelectedLanguage { get; private set; } // 0 - en, 1 - ru

    private const string LanguageKey = "Language"; 
    private static Dictionary<string, List<string>> localization;

    public static event LanguageChangeHandler OnLanguageChange;
    public delegate void LanguageChangeHandler();

    private TextAsset _textFile;
    private void Awake()
    {
        _textFile = Resources.Load("Localization") as TextAsset;
        Debug.Assert(_textFile, "No Localization xml in Resources folder");

        if (localization == null)
        {
            LoadLocalization();
        }
        AddLocalizedComponents();
    }
    private void Start()
    {
        GetLanguageAndSetLocalization();
    }

    protected virtual void GetLanguageAndSetLocalization()
    {
        if (!PlayerPrefs.HasKey(LanguageKey))
        {
            if (Application.systemLanguage == SystemLanguage.Russian)
            {
                PlayerPrefs.SetInt(LanguageKey, 1);
            }
            else
            {
                PlayerPrefs.SetInt(LanguageKey, 0);
            }
        }
        SetLanguage(PlayerPrefs.GetInt(LanguageKey));
    }

    private void AddLocalizedComponents()
    {
        var texts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            if (!text.GetComponent<LocalizedText>() &&
                !text.GetComponentInParent<TMP_Dropdown>() &&
                !text.text.Contains("IGNORELOCALIZE") &&
                !text.text.Equals(""))
            {
                text.gameObject.AddComponent<LocalizedText>();
            }

        }
    }
    public void SetLanguage(int id)
    {
        SelectedLanguage = id;
        OnLanguageChange?.Invoke();
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        PlayerPrefs.SetInt(LanguageKey, id);
        PlayerPrefs.Save();
#endif
    }
    private void LoadLocalization()
    {
        localization = new Dictionary<string, List<string>>();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(_textFile.text);
        foreach(XmlNode key in xmlDocument["Keys"].ChildNodes)
        {
            string keyString = key.Attributes["Name"].Value;
            var values = new List<string>();
            foreach (XmlNode translate in key["Translates"].ChildNodes)
            {
                values.Add(translate.InnerText);

                localization[keyString] = values;
            }
        }
    }
    public static string GetTranslate(string key, int languageId = -1)
    {
        if(languageId == -1)
        {
            languageId = SelectedLanguage;
        }
        if (localization.ContainsKey(key))
        {
            return localization[key][languageId];
        }
        //Debug.Log(key + " not found in localization file.");
        return key;
    }
}
