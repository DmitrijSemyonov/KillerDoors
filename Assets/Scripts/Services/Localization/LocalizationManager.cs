using KillerDoors.Services.AssetManagement;
using KillerDoors.Services.GameSettings;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace KillerDoors.Services.Localization
{
    public class LocalizationManager : ILocalizationService
    {
        public LanguageId SelectedLanguage { get; private set; }
        public bool IsInitialized { get; private set; }

        private const string LocalizationFilePath = "Localization";

        private Dictionary<string, List<string>> _localization;

        private TextAsset _localizationFile;

        public event Action LanguageChanged;

        private readonly IGameSettingsService _gameSettingsService;
        private readonly IAssetProvider _assetProvider;
        public LocalizationManager(IGameSettingsService gameSettingsService, IAssetProvider assetProvider)
        {
            _gameSettingsService = gameSettingsService;
            _assetProvider = assetProvider;
        }
        public void Init()
        {
            _localizationFile = _assetProvider.Load<TextAsset>(LocalizationFilePath);
            LoadLocalization();

            SelectedLanguage = (LanguageId)_gameSettingsService.SettingsData.languageId;
            IsInitialized = true;
            LanguageChanged?.Invoke();
        }
        public void SetLanguage(int id)
        {
            if (SelectedLanguage == (LanguageId)id) return;

            SelectedLanguage = (LanguageId)id;
            LanguageChanged?.Invoke();
            _gameSettingsService.SettingsData.languageId = id;
        }
        public string GetTranslate(string key, int languageId = -1)
        {
            if (!IsInitialized)
                return key;

            if (languageId == -1)
                languageId = (int)SelectedLanguage;

            if (_localization.ContainsKey(key))
                return _localization[key][languageId];

            Debug.LogWarning(key + " not found in localization file.");
            return key;
        }
        private void LoadLocalization()
        {
            _localization = new Dictionary<string, List<string>>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(_localizationFile.text);

            foreach (XmlNode key in xmlDocument["Keys"].ChildNodes)
            {
                string keyString = key.Attributes["Name"].Value;
                var values = new List<string>();

                foreach (XmlNode translate in key["Translates"].ChildNodes)
                    values.Add(translate.InnerText);

                _localization[keyString] = values;

            }
        }
    }
}