using KillerDoors.Services.CloseDoorModeDuration;
using KillerDoors.Services.DoorControl;
using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.ScoreSpace;
using KillerDoors.Services.TimeSpace;
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
        private readonly ITimeService _timeService;

        public CloseDoorState(IWindowsService windowsService, IScoreService scoreService, ICloseDoorsModeLimiter closeDoorsModeController,
            IDoorController doorController, ITimeService timeService)
        {
            _windowsService = windowsService;
            _scoreService = scoreService;
            _closeDoorsModeController = closeDoorsModeController;
            _doorController = doorController;
            _timeService = timeService;
        }
        public void Enter()
        {
            _windowsService.Close(WindowId.Ads);
            _windowsService.MainWindowOnScene.PlayCloseDoorsModeButton.HideButton();
            _windowsService.MainWindowOnScene.CloseDoorsButton.gameObject.SetActive(true);
            _closeDoorsModeController.StartGame();
            (_doorController as IUpdateListener).AddToUpdateList();
        }

        public void Exit()
        {
            _scoreService.EndDoorGame();
            _windowsService.Close(WindowId.Result);
            _windowsService.MainWindowOnScene.CloseDoorsButton.gameObject.SetActive(false);
            (_doorController as IUpdateListener).RemoveFromUpdateList();
            _timeService.ResetTimeScale();
        }

    }
}