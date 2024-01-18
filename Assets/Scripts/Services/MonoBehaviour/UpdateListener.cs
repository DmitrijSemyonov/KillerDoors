namespace KillerDoors.Services.MonoBehaviourFunctions
{
    public class UpdateListener : IUpdateListener
    {
        private IMonoBehaviourService _monoBehaviourService;

        public UpdateListener(IMonoBehaviourService monoBehaviourService) =>
            _monoBehaviourService = monoBehaviourService;

        public void AddToUpdateList() =>
            _monoBehaviourService.AddUpdateListener(this);

        public void RemoveFromUpdateList() => 
            _monoBehaviourService.RemoveUpdateListener(this);

        public void Tick() =>
            OnTick();

        protected virtual void OnTick() { }
    }
}