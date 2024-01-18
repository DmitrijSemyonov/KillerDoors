using KillerDoors.Services;
using KillerDoors.UI.StaticDataSpace;
using System;

namespace KillerDoors.UI.Windows
{
    public interface IWindowsService : IService
    {
        AdsWindow AdsWindowOnScene { get; }
        ShopWindow ShopWindowOnScene { get; set; }
        MainWindow MainWindowOnScene { get; }
        ResultCloseDoorGameWindow ResultCloseDoorGameWindowOnScene { get; }
        EducationWindow EducationWindowOnScene { get; }

        event Action<Window> WindowOpened;

        void Close(WindowId windowId);
        Window Open(WindowId windowID);
    }
}