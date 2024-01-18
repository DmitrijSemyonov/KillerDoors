using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.WebMediatorSpace;
using KillerDoors.StateMachine.States;
using KillerDoors.UI;
using System.Collections;
using UnityEngine;

namespace KillerDoors.InitGame
{
    public class GameBootstrap : MonoBehaviour
    {
        private Game _game;
        [SerializeField] private LoadingPanel _loadPanelPrefab;
        [SerializeField] private Yandex _yandex;
        void Awake()
        {
            IMonoBehaviourService moboBehaviourService = gameObject.AddComponent<MonoBehaviourService>();
            LoadingPanel loadPanel = Instantiate(_loadPanelPrefab);

            DontDestroyOnLoad(this);
            DontDestroyOnLoad(_yandex);
            DontDestroyOnLoad(loadPanel);

            loadPanel.BootstrapInit();

            if (_yandex) _yandex.SetLoadingPanel(loadPanel);

            _game = new Game(moboBehaviourService, loadPanel, _yandex);
            _game.gameStateMachine.Enter<BootstrapState>();
        }


    }
}