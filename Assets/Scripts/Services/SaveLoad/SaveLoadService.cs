using KillerDoors.Data;
using KillerDoors.Data.Extensions;
using KillerDoors.Services.GameSettings;
using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.WebMediatorSpace;
using KillerDoors.StateMachine;
using KillerDoors.StateMachine.States;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace KillerDoors.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "PlayerData";
        private const string SettingsKey = "SettingsData";
        private readonly IMonoBehaviourService _monoBehaviourService;
        private Coroutine _saveCoroutine;
        private IProgressService _progressService;
        private readonly IWebMediatorService _webMediatorService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameSettingsService _gameSettingsService;

        public SaveLoadService(IProgressService progressService, IGameSettingsService gameSettingsService, IWebMediatorService webMediatorService,
            IGameStateMachine gameStateMachine, IMonoBehaviourService monoBehaviourService)
        {
            _progressService = progressService;
            _webMediatorService = webMediatorService;
            _gameStateMachine = gameStateMachine;
            _gameSettingsService = gameSettingsService;
            _monoBehaviourService = monoBehaviourService;

            _progressService.DataChanged += DelayedSaveProgress;
            _gameSettingsService.DataChanged += SaveSettings;
        }

        public PlayerProgress LoadProgress() =>
            PlayerPrefs.GetString(ProgressKey)?.DeserializeTo<PlayerProgress>();
        public SettingsData LoadSettings() => 
            PlayerPrefs.GetString(SettingsKey)?.DeserializeTo<SettingsData>();
        public void CompareDataFromTheDeviceAndFromTheCloud(PlayerProgress dataFromTheCloud)
        {
            PlayerProgress deviceData = LoadProgress();
            if (deviceData == null || CountPoints(dataFromTheCloud) > CountPoints(deviceData))
            {
                _progressService.ProgressData = dataFromTheCloud;
                SaveProgressOnDevice();
                _gameStateMachine.Enter<LoadProgressState>();
            }
            else if (CountPoints(dataFromTheCloud) < CountPoints(deviceData))
            {
                SaveProgressOnCloud();
            }
        }
        public void DelayedSaveProgress()
        {
            if (_saveCoroutine != null)
                _monoBehaviourService.StopCoroutine(_saveCoroutine);

            _saveCoroutine = _monoBehaviourService.StartCoroutine(DelayedSaveProgressCoroutine());
        }
        private IEnumerator DelayedSaveProgressCoroutine()
        {
            yield return new WaitForSeconds(0.3f);
            SaveProgressOnDevice();
            SaveProgressOnCloud();
            _saveCoroutine = null;
        }
        public void SaveSettings()
        {
            PlayerPrefs.SetString(SettingsKey, _gameSettingsService.SettingsData.ToJson());
        }
        private void SaveProgressOnCloud()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        _webMediatorService.SavePlayerData(_progressService.ProgressData);
#endif
        }
        private void SaveProgressOnDevice() =>
            PlayerPrefs.SetString(ProgressKey, _progressService.ProgressData.ToJson());


        private int CountPoints(PlayerProgress data) =>
           (int)(Math.Pow(data.dinamit, 2) +
                Math.Pow(data.protection, 2) +
                data.scoreCoins +
                data.upgradesCoins.Sum());

    }
}