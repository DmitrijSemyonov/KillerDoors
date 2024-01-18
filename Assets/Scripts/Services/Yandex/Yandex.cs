using KillerDoors.Data;
using KillerDoors.Data.Extensions;
using KillerDoors.Services.Localization;
using KillerDoors.Services.SaveLoad;
using KillerDoors.UI;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace KillerDoors.Services.WebMediatorSpace
{
    public class Yandex : MonoBehaviour, IWebMediatorService
    {
#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void AuthorizationYG();

        [DllImport("__Internal")]
        private static extern void OpenAuthDialogYG();

        [DllImport("__Internal")]
        private static extern void SaveExtern(string data);

        [DllImport("__Internal")]
        private static extern void LoadExtern();
        [DllImport("__Internal")]
        private static extern string GetLanguage();
        [DllImport("__Internal")]
        private static extern string GetLanguageI18N();
        [DllImport("__Internal")]
        private static extern string ShowInterstitial();
        [DllImport("__Internal")]
        private static extern string ShowRewarded();
#endif
        private ISaveLoadService _saveLoadService;
        private ILocalizationService _localizationService;

        public bool IsSDKInitialized { get; private set; }
        public bool IsAuthorized { get; private set; }

        public event Action Authorized;

        public event Action AdsReceivedReward;
        public event Action RewardedAdsClosed;
        public event Action RewardedAdsEror;
        public event Action InterstitialAdsClosed;

        private LoadingPanel _loadingPanel;
        public void Construct(ISaveLoadService saveLoadService, ILocalizationService localizationService)
        {
            _saveLoadService = saveLoadService;
            _localizationService = localizationService;
        }
        public void SetLoadingPanel(LoadingPanel loadingPanel) =>
            _loadingPanel = loadingPanel;
        public void IdentifiedDesctop()
        {
            Application.runInBackground = false;
        }
        public void IdentifiedMobile()
        {
        }
        public void SendDataFromTheCloud(string jsonDataFromTheCloud)
        {
            PlayerProgress cloudData = jsonDataFromTheCloud.DeserializeTo<PlayerProgress>();
            StartCoroutine(WaitSaveLoadService(cloudData));
        }
        public void DefineLanguage()
        {
            string language = GetLanguageI18N();

            if (_loadingPanel != null)
                _loadingPanel.ApplyLanguagePreviousLocalization(language);

            StartCoroutine(WaitLocalizationService(language));
        }
        private IEnumerator WaitSaveLoadService(PlayerProgress cloudData)
        {
            while (_saveLoadService == null)
                yield return null;
            _saveLoadService.CompareDataFromTheDeviceAndFromTheCloud(cloudData);
        }
        private IEnumerator WaitLocalizationService(string language)
        {
            while (_localizationService == null ||
                !_localizationService.IsInitialized)
                yield return null;
            ApplyLanguage(language);
        }
        private void ApplyLanguage(string language)
        {
            if (language.Equals("ru"))
                _localizationService.SetLanguage((int)LanguageId.Ru);
            else
                _localizationService.SetLanguage((int)LanguageId.En);
        }
#if UNITY_WEBGL
        public void InformAuthorized()
        {
            IsAuthorized = true;
            LoadExtern();
            Authorized?.Invoke();
        }
        public void Authorization()
        {
            IsSDKInitialized = true;
            AuthorizationYG();
        }
        public void OpenAuthDialog()
        {
            OpenAuthDialogYG();
        }
        public void ShowInterstitialAds()
        {
            ShowInterstitial();
        }
        public void ShowRewardedAds()
        {
            ShowRewarded();
        }
        public string GetBrowserLanguage()
        {
            return GetLanguage();
        }
        public string Geti18NLanguage()
        {
            return GetLanguageI18N();
        }
        public void SavePlayerData(PlayerProgress data)
        {
            string jsonData = JsonUtility.ToJson(data);
            SaveExtern(jsonData);
        }
        public void OnAdsReceivedReward() =>
            AdsReceivedReward?.Invoke();
        public void OnRewardedAdsClosed() =>
            RewardedAdsClosed?.Invoke();
        public void OnRewardedAdsEror() =>
            RewardedAdsEror?.Invoke();
        public void OnInterstitialAdsClosed() =>
            InterstitialAdsClosed?.Invoke();
#endif
    }
}