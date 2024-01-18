using KillerDoors.UI.Factories;
using KillerDoors.UI.StaticDataSpace;
using System;

namespace KillerDoors.UI.Windows
{
    public class WindowsService : IWindowsService
    {
        private readonly IUIFactory _UIFactory;
        public AdsWindow AdsWindowOnScene { get; private set; }
        public ShopWindow ShopWindowOnScene { get; set; }
        public MainWindow MainWindowOnScene { get; private set; }
        public ResultCloseDoorGameWindow ResultCloseDoorGameWindowOnScene { get; private set; }
        public EducationWindow EducationWindowOnScene { get; private set; }

        public event Action<Window> WindowOpened;
        public WindowsService(IUIFactory iUIFactory)
        {
            _UIFactory = iUIFactory;
        }
        public Window Open(WindowId windowID)
        {
            Window window = null;
            switch (windowID)
            {
                case WindowId.None:
                    break;
                case WindowId.Shop:
                    window = _UIFactory.CreateShopWindow();
                    ShopWindowOnScene = window as ShopWindow;
                    break;
                case WindowId.Main:
                    window = _UIFactory.CreateMainWindow();
                    MainWindowOnScene = window as MainWindow;
                    break;
                case WindowId.Ads:
                    window = _UIFactory.CreateAdsWindow();
                    AdsWindowOnScene = window as AdsWindow;
                    break;
                case WindowId.Result:
                    window = _UIFactory.CreateResultWindow();
                    ResultCloseDoorGameWindowOnScene = window as ResultCloseDoorGameWindow;
                    break;
                case WindowId.Education:
                    window = _UIFactory.CreateEducationWindow();
                    EducationWindowOnScene = window as EducationWindow;
                    break;
            }
            WindowOpened?.Invoke(window);
            SortWindows(window);
            return window;
        }
        public void Close(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.None:
                    break;
                case WindowId.Main:
                    if (MainWindowOnScene)
                        MainWindowOnScene.Close();
                    break;
                case WindowId.Shop:
                    if (ShopWindowOnScene)
                        ShopWindowOnScene.Close();
                    break;
                case WindowId.Ads:
                    if (AdsWindowOnScene)
                        AdsWindowOnScene.Close();
                    break;
                case WindowId.Result:
                    if (ResultCloseDoorGameWindowOnScene)
                        ResultCloseDoorGameWindowOnScene.Close();
                    break;
                case WindowId.Education:
                    if (EducationWindowOnScene)
                        EducationWindowOnScene.Close();
                    break;
            }
        }
        private void SortWindows(Window createdWindow)
        {
            if (EducationWindowOnScene
                && EducationWindowOnScene.transform.GetSiblingIndex() < createdWindow.transform.GetSiblingIndex())
            {
                EducationWindowOnScene.transform.SetAsLastSibling();
            }
        }
    }
}