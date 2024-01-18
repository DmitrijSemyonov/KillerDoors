using KillerDoors.Data;
using KillerDoors.Services.GameSettings;
using KillerDoors.Services.Localization;
using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.SaveLoad;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.UI;
using UnityEngine;

namespace KillerDoors.StateMachine.States
{
    public class LoadProgressState : IState
    {
        private const string GameLevel = "Game";
        private readonly GameStateMachine _gameStateMachine;
        private readonly LoadingPanel _loadingPanel;
        private readonly IProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataService _staticDataService;
        private readonly IMonoBehaviourService _monoBehaviourService;
        private readonly IGameSettingsService _gameSettingsService;
        private readonly ISoundService _soundService;
        private readonly ILocalizationService _localizationService;

        public LoadProgressState(GameStateMachine gameStateMachine, LoadingPanel loadingPanel, IProgressService progressService, ISaveLoadService saveLoadService,
            IStaticDataService staticDataService, IMonoBehaviourService monoBehaviourService, IGameSettingsService gameSettingsService, ISoundService soundService, 
            ILocalizationService localizationService)
        {
            _gameStateMachine = gameStateMachine;
            _loadingPanel = loadingPanel;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _staticDataService = staticDataService;
            _monoBehaviourService = monoBehaviourService;
            _gameSettingsService = gameSettingsService;
            _soundService = soundService;
            _localizationService = localizationService;
        }

        public void Enter()
        {
            if (_progressService.ObservableProgress != null)
                _progressService.ObservableProgress.Dispose();

            _loadingPanel.gameObject.SetActive(true);
            _loadingPanel.AppearanceDisappearance.transform.localScale = new Vector3(1f, 1f, 1f);

            LoadProgressOrCreate();

            _gameStateMachine.Enter<LoadLevelState, string>(GameLevel);
        }

        private void LoadProgressOrCreate()
        {
            _progressService.ProgressData = _saveLoadService.LoadProgress();

            _progressService.Init(_staticDataService);

            _gameSettingsService.SettingsData = _saveLoadService.LoadSettings() ?? new SettingsData();

            _localizationService.Init();

            _soundService.Init();
        }

        public void Exit()
        {
        }
    }
}