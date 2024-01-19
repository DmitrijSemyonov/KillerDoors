using Helpers;
using KillerDoors.Common;
using KillerDoors.Services.CloseDoorModeDuration;
using KillerDoors.Services.DoorControl;
using KillerDoors.Services.Education;
using KillerDoors.Services.Factories;
using KillerDoors.Services.GameSounds;
using KillerDoors.Services.Merge;
using KillerDoors.Services.PersonSpawn;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.SceneLoad;
using KillerDoors.Services.ScoreSpace;
using KillerDoors.Services.ShopSpace;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.Services.UpgradeDoor;
using KillerDoors.StaticDataSpace;
using KillerDoors.UI;
using KillerDoors.UI.Factories;
using KillerDoors.UI.SpotAppearanceSpace;
using KillerDoors.UI.StaticDataSpace;
using KillerDoors.UI.Windows;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace KillerDoors.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingPanel _loadingPanel;
        private readonly IDoorFactory _doorFactory;
        private readonly IProgressService _progressService;
        private readonly IDoorController _doorController;
        private readonly IMergeService _mergeService;
        private readonly IScoreService _scoreService;
        private readonly IStaticDataService _staticDataService;
        private readonly ILosingZoneFactory _losingZoneFactory;
        private readonly IUIFactory _UIFactory;
        private readonly IDoorUpgradeService _doorUpgradeService;
        private readonly IShopService _shop;
        private readonly IWindowsService _windowsService;
        private readonly IPersonSpawner _personSpawner;
        private readonly ICloseDoorsModeLimiter _closeDoorsModeController;
        private readonly ISpotAppearanceViewService _spotAppearanceViewService;
        private readonly IEducationService _educationService;
        private readonly ISounds _sounds;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingPanel loadingPanel, IDoorFactory doorFactory,
            IProgressService progressService, IDoorController doorController, IMergeService mergeService, IScoreService scoreService,
            IStaticDataService staticDataService, ILosingZoneFactory losingZoneFactory, IUIFactory uIFactory, IDoorUpgradeService doorUpgradeService,
            IShopService shop, IWindowsService windowsService, IPersonSpawner personSpawner, ICloseDoorsModeLimiter closeDoorsModeController,
            ISpotAppearanceViewService spotAppearanceViewService, IEducationService educationService, ISounds sounds)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingPanel = loadingPanel;
            _doorFactory = doorFactory;
            _progressService = progressService;
            _doorController = doorController;
            _mergeService = mergeService;
            _scoreService = scoreService;
            _staticDataService = staticDataService;
            _losingZoneFactory = losingZoneFactory;
            _UIFactory = uIFactory;
            _doorUpgradeService = doorUpgradeService;
            _shop = shop;
            _windowsService = windowsService;
            _personSpawner = personSpawner;
            _closeDoorsModeController = closeDoorsModeController;
            _spotAppearanceViewService = spotAppearanceViewService;
            _educationService = educationService;
            _sounds = sounds;
        }
        public void Enter(string name)
        {
            _sceneLoader.SceneLoad(name, OnSceleLoad);
        }

        private void OnSceleLoad()
        {
            LevelStaticData levelData = _staticDataService.ForLevel(SceneManager.GetActiveScene().name);

            List<Door> doors = _doorFactory.CreateDoors();

            LosingZone losingZone = _losingZoneFactory.CreateLosingZone(levelData.looseTriggerPosition);

            _progressService.ObservableProgress.InitForLevel(SceneManager.GetActiveScene().name);

            losingZone.Construct(_progressService.ObservableProgress);

            _closeDoorsModeController.Init(losingZone);

            _shop.Init();

            _doorController.Init(doors);

            _doorUpgradeService.Init(doors);

            _UIFactory.CreateUIRoot();
            _windowsService.Open(WindowId.Main);

            _scoreService.Init(losingZone, doors, _windowsService.MainWindowOnScene.Inventory);

            _mergeService.Init(_windowsService.MainWindowOnScene.Inventory);

            _windowsService.MainWindowOnScene.Inventory.Init();

            _personSpawner.Init();

            _spotAppearanceViewService.Init(losingZone);

            _educationService.Init();

            _sounds.Init(doors);

            _gameStateMachine.Enter<MergeCoinsState>();
        }


        public void Exit()
        {
            _loadingPanel.GetComponentInChildren<AppearingAndDisappearingObject>().StartDisappearing();
        }
    }
}