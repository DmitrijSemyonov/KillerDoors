using KillerDoors.Services;
using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.SceneLoad;
using KillerDoors.Services.WebMediatorSpace;
using KillerDoors.StateMachine;
using KillerDoors.UI;

namespace KillerDoors.InitGame
{
    public class Game
    {
        public GameStateMachine gameStateMachine;

        public Game(IMonoBehaviourService coroutineRunner, LoadingPanel loadingPanel, IWebMediatorService webMediatorService)
        {
            SceneLoader sceneLoader = new SceneLoader(coroutineRunner);
            gameStateMachine = new GameStateMachine(sceneLoader, loadingPanel, ServiceLocator.Container, coroutineRunner, webMediatorService);
        }
    }
}