using KillerDoors.Common;

namespace KillerDoors.Services.CloseDoorModeDuration
{
    public interface ICloseDoorsModeLimiter : IService
    {
        void Describes();
        void Init(LosingZone losingZone);
        void StartGame();
    }
}