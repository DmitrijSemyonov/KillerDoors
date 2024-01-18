namespace KillerDoors.Services.TimeSpace
{
    public interface ITimeService : IService
    {
        void Pause();
        void ResetTimeScale();
        void Resume();
        void SetTimeScale(float newValue);
    }
}