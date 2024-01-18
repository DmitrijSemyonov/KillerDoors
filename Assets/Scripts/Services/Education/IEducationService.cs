namespace KillerDoors.Services.Education
{
    public interface IEducationService : IService
    {
        void ContinueAfterReading();
        void Init();
        void OnEducationCompleted();
    }
}