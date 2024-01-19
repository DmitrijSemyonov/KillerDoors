using KillerDoors.Services;
using KillerDoors.Services.CloseDoorModeDuration;
using KillerDoors.Services.DoorControl;
using KillerDoors.Services.Education;
using KillerDoors.Services.Factories;
using KillerDoors.Services.GameSettings;
using KillerDoors.Services.GameSounds;
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
using KillerDoors.Services.WebMediatorSpace;
using KillerDoors.StateMachine.States;
using KillerDoors.UI;
using KillerDoors.UI.Factories;
using KillerDoors.UI.SpotAppearanceSpace;
using KillerDoors.UI.Windows;
using System;
using System.Collections.Generic;

namespace KillerDoors.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingPanel loadingPanel, ServiceLocator services, IMonoBehaviourService coroutineRunner,
            IWebMediatorService webMediatorService)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services, webMediatorService, coroutineRunner),

                [typeof(LoadProgressState)] = new LoadProgressState(this, loadingPanel, services.Single<IProgressService>(), services.Single<ISaveLoadService>(),
                services.Single<IStaticDataService>(), coroutineRunner, services.Single<IGameSettingsService>(), services.Single<ISoundService>(),
                services.Single<ILocalizationService>()),

                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingPanel, services.Single<IDoorFactory>(), services.Single<IProgressService>(),
                services.Single<IDoorController>(), services.Single<IMergeService>(), services.Single<IScoreService>(), services.Single<IStaticDataService>(),
                services.Single<ILosingZoneFactory>(), services.Single<IUIFactory>(), services.Single<IDoorUpgradeService>(), services.Single<IShopService>(),
                services.Single<IWindowsService>(), services.Single<IPersonSpawner>(), services.Single<ICloseDoorsModeLimiter>(),
                services.Single<ISpotAppearanceViewService>(), services.Single<IEducationService>(), services.Single<ISounds>()),

                [typeof(MergeCoinsState)] = new MergeCoinsState(services.Single<IWindowsService>()),

                [typeof(CloseDoorState)] = new CloseDoorState(services.Single<IWindowsService>(), services.Single<IScoreService>(),
                services.Single<ICloseDoorsModeLimiter>(), services.Single<IDoorController>(), services.Single<ITimeService>()),
            };
        }

        public void Enter<T>() where T : class, IState
        {
            T state = ChangeState<T>();
            state.Enter();
        }
        public void Enter<T, TPayload>(TPayload payload) where T : class, IPayloadedState<TPayload>
        {
            T state = ChangeState<T>();
            state.Enter(payload);
        }
        private T ChangeState<T>() where T : class, IExitableState
        {
            _currentState?.Exit();
            T state = GetState<T>();
            _currentState = state;
            return state;
        }
        private T GetState<T>() where T : class, IExitableState =>
            _states[typeof(T)] as T;
    }
}