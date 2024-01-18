using KillerDoors.Services.DoorControl;
using KillerDoors.Services.GameSettings;
using KillerDoors.Services.Merge;
using KillerDoors.Services.PersonSpawn;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.WebMediatorSpace;
using KillerDoors.StateMachine;
using KillerDoors.UI.Common;
using KillerDoors.UI.Common.ChangeGameState;
using KillerDoors.UI.InventorySpace;
using KillerDoors.UI.Windows.Main.Views;
using UnityEngine;

namespace KillerDoors.UI.Windows
{
    public class MainWindow : Window
    {
        [SerializeField] private PointsView _pointsView;
        [SerializeField] private DinamitView _dinamitView;
        [SerializeField] private SoundView _soundView;
        [SerializeField] private ProtectionView _protectionView;
        [SerializeField] private AuthorizationButton _authorizationButton;
        [field: SerializeField] public ScoreCoinsView ScoreCoinsView { get; private set; }
        [field: SerializeField] public CloseDoorsButton CloseDoorsButton { get; private set; }
        [field: SerializeField] public Inventory Inventory { get; private set; }
        [field: SerializeField] public ChangeGameStateButton PlayCloseDoorsModeButton { get; private set; }

        public void Construct(IProgressService progress, IWindowsService windowService, ISoundService soundService,
            IWebMediatorService webMediatorService, IDoorController doorController, IGameStateMachine gameStateMachine, IMergeService mergeService,
            IGameSettingsService gameSettingsService, IPersonSpawner personSpawner, Transform UIRoot)
        {
            ScoreCoinsView.Construct(progress.ObservableProgress, windowService, PlayCloseDoorsModeButton);
            _pointsView.Construct(progress.ObservableProgress);
            _dinamitView.Construct(progress.ObservableProgress, personSpawner);
            _protectionView.Construct(progress.ObservableProgress);
            _soundView.Construct(soundService, gameSettingsService);
            _authorizationButton.Construct(webMediatorService);
            CloseDoorsButton.Construct(doorController);
            Inventory.Construct(mergeService, progress, UIRoot);
            PlayCloseDoorsModeButton.Construct(gameStateMachine);
        }
        public void Subscribes()
        {
            ScoreCoinsView.Subscribes();
            _pointsView.Subscribes();
            _dinamitView.Subscribes();
            _protectionView.Subscribes();
        }
    }
}