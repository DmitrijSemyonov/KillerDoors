using KillerDoors.Services;
using KillerDoors.Services.Education;
using KillerDoors.UI.Windows.Education.Views;
using KillerDoors.UI.SpotAppearanceSpace;
using KillerDoors.UI.Windows;
using UnityEngine;

namespace KillerDoors.UI.Factories
{
    public interface IUIFactory : IService
    {
        MainWindow CreateMainWindow();
        ShopWindow CreateShopWindow();
        void CreateUIRoot();
        AdsWindow CreateAdsWindow();
        ResultCloseDoorGameWindow CreateResultWindow();
        SpotAppearanceView CreateSpotAppearanceView(Vector3 cameraPosition);
        EducationWindow CreateEducationWindow();
        FingerView CreateFingerView();
        void Construct2(IWindowsService windowService, IEducationService educationService);
    }
}