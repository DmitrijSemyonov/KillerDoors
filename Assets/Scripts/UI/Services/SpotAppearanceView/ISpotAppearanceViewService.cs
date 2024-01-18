using KillerDoors.Common;
using KillerDoors.Services;

namespace KillerDoors.UI.SpotAppearanceSpace
{
    public interface ISpotAppearanceViewService : IService
    {
        void DescribesSceneObjects();
        void Init(LosingZone losingZone);
        void SubscribeSceneObjects();
    }
}