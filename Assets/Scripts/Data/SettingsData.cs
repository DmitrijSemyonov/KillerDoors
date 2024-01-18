using KillerDoors.Services.Localization;
using System;
using UnityEngine;

namespace KillerDoors.Data
{
    [Serializable]
    public class SettingsData
    {
        public bool isSoundEnabled = true;
        public int languageId;

        public SettingsData()
        {
            if (Application.systemLanguage == SystemLanguage.Russian)
                languageId = (int)LanguageId.Ru;
            else
                languageId = (int)LanguageId.En;
        }
    }
}