using KillerDoors.UI.StaticDataSpace;
using KillerDoors.UI.Windows;

namespace KillerDoors.StateMachine.States
{
    public class MergeCoinsState : IState
    {
        private readonly IWindowsService _windowsService;

        public MergeCoinsState(IWindowsService windowsService)
        {
            _windowsService = windowsService;
        }

        public void Enter()
        {
            _windowsService.Open(WindowId.Ads);
            _windowsService.MainWindowOnScene.PlayCloseDoorsModeButton.ShowButton();
        }

        public void Exit()
        {
        }
    }
}