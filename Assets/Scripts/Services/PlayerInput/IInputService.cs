namespace KillerDoors.Services.InputSpace
{
    public interface IInputService : IService
    {
        public bool IsCloseDoorPressed { get; }
        public bool IsDinamitPressed { get; }

    }
}