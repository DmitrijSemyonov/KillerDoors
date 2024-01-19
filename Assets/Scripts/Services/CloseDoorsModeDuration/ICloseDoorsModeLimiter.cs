using KillerDoors.Common;

namespace KillerDoors.Services.CloseDoorModeDuration
{
    public interface ICloseDoorsModeLimiter : IService
    {
        void Init(LosingZone losingZone);
        void StartGame();
    }
}