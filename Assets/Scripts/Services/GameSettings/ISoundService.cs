namespace KillerDoors.Services.GameSettings
{
    public interface ISoundService : IService
    {
        void Init();
        void TurnTheAudioSwitch(bool value);
    }
}