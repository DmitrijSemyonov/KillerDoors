using KillerDoors.Services;
using KillerDoors.Services.AdsSpace;
using KillerDoors.Services.AssetManagement;
using KillerDoors.Services.CloseDoorModeDuration;
using KillerDoors.Services.DoorControl;
using KillerDoors.Services.Education;
using KillerDoors.Services.Factories;
using KillerDoors.Services.GameSettings;
using KillerDoors.Services.GameSounds;
using KillerDoors.Services.InputSpace;
using KillerDoors.Services.Localization;
using KillerDoors.Services.Merge;
using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.PersonSpawn;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.SaveLoad;
using KillerDoors.Services.SceneLoad;
using KillerDoors.Services.ScoreSpace;
using KillerDoors.Services.ShopSpace;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.Services.TimeSpace;
using KillerDoors.Services.UpgradeDoor;
using KillerDoors.Services.VFX;
using KillerDoors.Services.WebMediatorSpace;
using KillerDoors.UI.Factories;
using KillerDoors.UI.SpotAppearanceSpace;
using KillerDoors.UI.Windows;
using System.Collections;

namespace KillerDoors.StateMachine.States
{
    public class BootstrapState : IState
    {
        private const string InitScene = "InitScene";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ServiceLocator _services;
        private readonly IWebMediatorService _webMediatorService;
        private readonly IMonoBehaviourService _monoBehaviourService;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, ServiceLocator services, IWebMediatorService yandex,
            IMonoBehaviourService coroutineRunner)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _webMediatorService = yandex;
            _monoBehaviourService = coroutineRunner;
            RegisterServices();
        }
        public void Enter()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        _monoBehaviourService.StartCoroutine(WaitForYandexSDKInitialized());
        return;
#endif
            _sceneLoader.SceneLoad(InitScene, onCompleteLoad: LoadLevel);
        }
        private IEnumerator WaitForYandexSDKInitialized()
        {
            while (!_webMediatorService.IsSDKInitialized)
                yield return null;
            _sceneLoader.SceneLoad(InitScene, onCompleteLoad: LoadLevel);
        }
        private void LoadLevel()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }
        private void RegisterServices()
        {
            _services.RegisterSingle<IGameStateMachine>(_gameStateMachine);

            _services.RegisterSingle<IMonoBehaviourService>(_monoBehaviourService);

            _services.RegisterSingle<ISceneLoadService>(_sceneLoader);

            IProgressService progressService = new ProgressService();
            _services.RegisterSingle(progressService);

            IGameSettingsService gameSettingsService = new GameSettingsService();
            _services.RegisterSingle(gameSettingsService);

            ISaveLoadService saveLoadService = new SaveLoadService(progressService, gameSettingsService, _webMediatorService, _gameStateMachine,
                _monoBehaviourService);
            _services.RegisterSingle(saveLoadService);

            IAssetProvider assetProvider = new AssetProvider();
            _services.RegisterSingle(assetProvider);

            ILocalizationService localizationService = new LocalizationManager(gameSettingsService, assetProvider);
            _services.RegisterSingle(localizationService);

            if (_webMediatorService != null)
            {
                _webMediatorService.Construct(saveLoadService, localizationService);
                _services.RegisterSingle<IWebMediatorService> (_webMediatorService);
            }

            IInputService input = new StandaloneInputService();
            _services.RegisterSingle(input);


            IStaticDataService staticDataService = new StaticDataService();
            staticDataService.Load();
            _services.RegisterSingle(staticDataService);

            IAdsService adsService = new Ads(_webMediatorService, staticDataService);
            _services.RegisterSingle(adsService);

            ITimeService timeService = new TimeService(_monoBehaviourService);
            _services.RegisterSingle(timeService);

            IDoorFactory doorFactory = new DoorFactory(assetProvider, staticDataService);
            _services.RegisterSingle(doorFactory);

            ILosingZoneFactory losingZoneFactory = new LosingZoneFactory(assetProvider);
            _services.RegisterSingle(losingZoneFactory);

            IScoreService scoreService = new ScoreService(progressService);
            _services.RegisterSingle(scoreService);

            IDoorController doorController = new DoorController(_monoBehaviourService, input);
            _services.RegisterSingle(doorController);

            IMergeService mergeService = new MergeController(progressService, scoreService, staticDataService);
            _services.RegisterSingle(mergeService);

            ISoundService soundService = new SoundService(assetProvider, _monoBehaviourService, gameSettingsService, adsService);
            _services.RegisterSingle(soundService);

            IDoorUpgradeService doorUpgradeService = new DoorUpgrade(staticDataService, progressService);
            _services.RegisterSingle(doorUpgradeService);

            IShopService shopService = new Shop(progressService, staticDataService);
            _services.RegisterSingle(shopService);

            IAdsRewardService adsRewardService = new AdsRewardService(adsService, progressService, scoreService, staticDataService, timeService);
            _services.RegisterSingle(adsRewardService);

            IPersonFactory personFactory = new PersonFactory(assetProvider, staticDataService);
            _services.RegisterSingle(personFactory);

            IPersonSpawner personSpawner = new PersonSpawner(_monoBehaviourService, staticDataService, personFactory, progressService, scoreService, input);
            _services.RegisterSingle(personSpawner);

            IUIFactory UIFactory = new UIFactory(assetProvider, progressService, staticDataService, adsService, soundService, _webMediatorService,
                doorController, _gameStateMachine, mergeService, gameSettingsService, doorUpgradeService, shopService, adsRewardService, personSpawner, scoreService);
            _services.RegisterSingle(UIFactory);

            IWindowsService windowService = new WindowsService(UIFactory);
            _services.RegisterSingle(windowService);

            IEducationService educationService = new EducationService(mergeService, progressService, windowService, timeService, UIFactory, _monoBehaviourService);
            _services.RegisterSingle(educationService);

            adsRewardService.Construct2(windowService);

            UIFactory.Construct2(windowService, educationService);

            ICloseDoorsModeLimiter closeDoorsModeController = new CloseDoorsModeLimiter(staticDataService, personSpawner, windowService);
            _services.RegisterSingle(closeDoorsModeController);

            ISpotAppearanceViewService spotAppearanceViewService = new SpotAppearanceViewService(UIFactory, mergeService, scoreService, _monoBehaviourService,
                staticDataService);
            _services.RegisterSingle(spotAppearanceViewService);

            ISoundsFactory soundsFactory = new SoundsFactory(assetProvider);
            _services.RegisterSingle(soundsFactory);

            ISounds sounds = new Sounds(mergeService, soundsFactory);
            _services.RegisterSingle(sounds);

            IVFXFactory VFXFactory = new VFXFactory(assetProvider);
            _services.RegisterSingle(VFXFactory);

            IVFXService VFXService = new VFXService(VFXFactory, staticDataService, scoreService);
            _services.RegisterSingle(VFXService);
        }

        public void Exit()
        {
        }
    }
}