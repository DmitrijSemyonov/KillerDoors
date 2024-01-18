using Helpers;
using KillerDoors.StateMachine;
using KillerDoors.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;

namespace KillerDoors.UI.Common.ChangeGameState
{
    public class ChangeGameStateButton : MonoBehaviour
    {
        [SerializeField] private GameStateId _gameStateId;
        [field: SerializeField] public Button Button { get; private set; }
        [SerializeField] private AppearingAndDisappearingObject _appearingAnimation;

        private IGameStateMachine _gameStateMachine;

        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            Button.onClick.AddListener(ChangeGameState);
        }
        public void ShowButton() => 
            _appearingAnimation.StartAppearing();
        public void HideButton() =>
            _appearingAnimation.StartDisappearing();

        private void ChangeGameState()
        {
            switch (_gameStateId)
            {
                case GameStateId.None:
                    break;
                case GameStateId.MergeCoins:
                    _gameStateMachine.Enter<MergeCoinsState>();
                    break;
                case GameStateId.CloseDoors:
                    _gameStateMachine.Enter<CloseDoorState>();
                    break;
            }
        }
    }
}