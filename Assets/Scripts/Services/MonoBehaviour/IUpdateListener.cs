namespace KillerDoors.Services.MonoBehaviourFunctions
{
    public interface IUpdateListener
    {
        public void AddToUpdateList();
        public void RemoveFromUpdateList();
        public void Tick();
    }
}