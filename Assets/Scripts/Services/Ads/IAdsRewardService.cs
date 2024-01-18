using KillerDoors.UI.Windows;

namespace KillerDoors.Services.AdsSpace
{
    public interface IAdsRewardService : IService
    {
        void AdsRequest(AdsId forItem);
        void Construct2(IWindowsService windowsService);
    }
}