using KillerDoors.Services;
using KillerDoors.StateMachine.States;

namespace KillerDoors.StateMachine
{
    public interface IGameStateMachine : IService
    {
        void Enter<T>() where T : class, IState;
        void Enter<T, TPayload>(TPayload payload) where T : class, IPayloadedState<TPayload>;
    }
}