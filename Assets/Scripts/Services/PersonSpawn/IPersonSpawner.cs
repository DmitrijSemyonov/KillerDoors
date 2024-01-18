namespace KillerDoors.Services.PersonSpawn
{
    public interface IPersonSpawner : IService
    {
        void TryExploseAll();
        void StartSpawn();
        void StopSpawnAndKill();
        void Init();
    }
}