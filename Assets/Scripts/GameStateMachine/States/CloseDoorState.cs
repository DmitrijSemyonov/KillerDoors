using KillerDoors.Services.CloseDoorModeDuration;
using KillerDoors.Services.DoorControl;
using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.ScoreSpace;
using KillerDoors.Services.TimeSpace;
using KillerDoors.UI.SpotAppearanceSpace;
using KillerDoors.UI.StaticDataSpace;
using KillerDoors.UI.Windows;

namespace KillerDoors.StateMachine.States
{
    public class CloseDoorState : IState
    {
        private readonly IWindowsService _windowsService;
        private readonly IScoreService _scoreService;
        private readonly ICloseDoorsModeLimiter _closeDoorsModeController;
        private readonly IDoorController _doorController;
        private readonly ISpotAppearanceViewService _spotAppearanceViewService;
        private readonly ITimeService _timeService;

        public CloseDoorState(IWindowsService windowsService, IScoreService scoreService, ICloseDoorsModeLimiter closeDoorsModeController,
            IDoorController doorController, ISpotAppearanceViewService spotAppearanceViewService, ITimeService timeService)
        {
            _windowsService = windowsService;
            _scoreService = scoreService;
            _closeDoorsModeController = closeDoorsModeController;
            _doorController = doorController;
            _spotAppearanceViewService = spotAppearanceViewService;
            _timeService = timeService;
        }
        public void Enter()
        {
            _windowsService.Close(WindowId.Ads);
            _scoreService.Subscribes();
            _spotAppearanceViewService.SubscribeSceneObjects();
            _windowsService.MainWindowOnScene.PlayCloseDoorsModeButton.HideButton();
            _windowsService.MainWindowOnScene.CloseDoorsButton.gameObject.SetActive(true);
            _closeDoorsModeController.StartGame();
            (_doorController as IUpdateListener).AddToUpdateList();
        }

        public void Exit()
        {
            _closeDoorsModeController.Describes();
            _scoreService.EndDoorGame();
            _scoreService.Describes();
            _spotAppearanceViewService.DescribesSceneObjects();
            _windowsService.Close(WindowId.Result);
            _windowsService.MainWindowOnScene.CloseDoorsButton.gameObject.SetActive(false);
            (_doorController as IUpdateListener).RemoveFromUpdateList();
            _timeService.ResetTimeScale();
        }

    }
}