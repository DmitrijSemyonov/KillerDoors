using KillerDoors.Services.AdsSpace;
using KillerDoors.Services.AssetManagement;
using KillerDoors.Services.DoorControl;
using KillerDoors.Services.Education;
using KillerDoors.Services.GameSettings;
using KillerDoors.Services.Merge;
using KillerDoors.Services.PersonSpawn;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.ScoreSpace;
using KillerDoors.Services.ShopSpace;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.Services.UpgradeDoor;
using KillerDoors.Services.WebMediatorSpace;
using KillerDoors.StateMachine;
using KillerDoors.UI.Windows.Education.Views;
using KillerDoors.UI.SpotAppearanceSpace;
using KillerDoors.UI.StaticDataSpace;
using KillerDoors.UI.Windows;
using UnityEngine;

namespace KillerDoors.UI.Factories
{
    public class UIFactory : IUIFactory
    {
        private const string UIReootPath = "UI/UIRoot";
        private const string SpotAppearanceViewPath = "UI/SpotAppearanceView";
        private const string FingerViewPath = "UI/FingerView";
        private Transform _UIRoot;

        private readonly IAssetProvider _assetProvider;
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly IAdsService _adsService;
        private IWindowsService _windowService;
        private IEducationService _educationService;
        private readonly IAdsRewardService _adsRewardService;
        private readonly IDoorUpgradeService _doorUpgradeService;
        private readonly ISoundService _soundService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IWebMediatorService _webMediatorService;
        private readonly IDoorController _doorController;
        private readonly IMergeService _mergeService;
        private readonly IGameSettingsService _gameSettingsService;
        private readonly IShopService _shopService;
        private readonly IPersonSpawner _personSpawner;
        private readonly IScoreService _scoreService;

        public UIFactory(IAssetProvider assetProvider, IProgressService progressService, IStaticDataService staticDataService, IAdsService adsService,
            ISoundService soundService, IWebMediatorService webMediatorService, IDoorController doorController, IGameStateMachine gameStateMachine,
            IMergeService mergeService, IGameSettingsService gameSettingsService, IDoorUpgradeService doorUpgradeService, IShopService shopService,
            IAdsRewardService adsRewardService, IPersonSpawner personSpawner, IScoreService scoreService)
        {
            _assetProvider = assetProvider;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _adsService = adsService;
            _soundService = soundService;
            _gameStateMachine = gameStateMachine;
            _webMediatorService = webMediatorService;
            _doorController = doorController;
            _mergeService = mergeService;
            _gameSettingsService = gameSettingsService;
            _doorUpgradeService = doorUpgradeService;
            _shopService = shopService;
            _adsRewardService = adsRewardService;
            _personSpawner = personSpawner;
            _scoreService = scoreService;
        }
        public void Construct2(IWindowsService windowService, IEducationService educationService)
        {
            _windowService = windowService;
            _educationService = educationService;
        }
        public void CreateUIRoot()
        {
            GameObject ui = _assetProvider.Instantiate<GameObject>(UIReootPath);
            _UIRoot = ui.transform;
        }
        public MainWindow CreateMainWindow()
        {
            WindowConfig config = _staticDataService.ForWindow(WindowId.Main);
            MainWindow main = Object.Instantiate(config.prefab, _UIRoot) as MainWindow;
            main.Construct(_progressService, _windowService, _soundService, _webMediatorService, _doorController, _gameStateMachine, _mergeService,
                _gameSettingsService, _personSpawner, _UIRoot);
            main.Subscribes();
            return main;
        }
        public ShopWindow CreateShopWindow()
        {
            WindowConfig config = _staticDataService.ForWindow(WindowId.Shop);
            ShopWindow shop = Object.Instantiate(config.prefab, _UIRoot) as ShopWindow;
            shop.Construct(_progressService.ObservableProgress, _staticDataService, _doorUpgradeService, _assetProvider, _shopService);
            shop.Init();
            shop.Subscribes();
            return shop;
        }
        public AdsWindow CreateAdsWindow()
        {
            WindowConfig config = _staticDataService.ForWindow(WindowId.Ads);
            AdsWindow ads = Object.Instantiate(config.prefab, _UIRoot) as AdsWindow;
            ads.Construct(_adsRewardService);
            return ads;
        }
        public ResultCloseDoorGameWindow CreateResultWindow()
        {
            WindowConfig config = _staticDataService.ForWindow(WindowId.Result);
            ResultCloseDoorGameWindow window = Object.Instantiate(config.prefab, _UIRoot) as ResultCloseDoorGameWindow;
            window.Construct(_adsRewardService, _scoreService, _gameStateMachine, _staticDataService);
            window.Init();
            return window;
        }
        public EducationWindow CreateEducationWindow()
        {
            WindowConfig config = _staticDataService.ForWindow(WindowId.Education);
            EducationWindow window = Object.Instantiate(config.prefab, _UIRoot) as EducationWindow;
            window.Construct(_educationService);
            return window;
        }
        public FingerView CreateFingerView()
        {
            FingerView fingerAnimation = _assetProvider.Instantiate<FingerView>(FingerViewPath, _UIRoot);
            fingerAnimation.Construct(_staticDataService);
            return fingerAnimation;
        }
        public SpotAppearanceView CreateSpotAppearanceView(Vector3 cameraPosition)
        {
            SpotAppearanceView view = _assetProvider.Instantiate<SpotAppearanceView>(SpotAppearanceViewPath, cameraPosition, Quaternion.identity, _UIRoot);
            return view;
        }
    }
}