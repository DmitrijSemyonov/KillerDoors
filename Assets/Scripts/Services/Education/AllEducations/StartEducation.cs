using KillerDoors.Services.Merge;
using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.TimeSpace;
using KillerDoors.UI.Windows.Education.Views;
using KillerDoors.UI.Factories;
using KillerDoors.UI.InventorySpace;
using KillerDoors.UI.StaticDataSpace;
using KillerDoors.UI.Windows;
using System.Collections.Generic;
using UnityEngine;

namespace KillerDoors.Services.Education
{
    public class StartEducation : EducationBase
    {
        private readonly IProgressService _progressService;
        private readonly IUIFactory _UIFactory;
        private readonly IMergeService _mergeController;

        private FingerView _fingerView;


        public StartEducation(IEducationService educationService, IMergeService mergeService, IProgressService progressService, IWindowsService windowsService,
            ITimeService timeService, IUIFactory UIFactory, IMonoBehaviourService monoBehaviourService)
            : base(educationService, windowsService, timeService, monoBehaviourService)
        {
            _mergeController = mergeService;
            _progressService = progressService;
            _UIFactory = UIFactory;
        }

        public override void Init()
        {
            _windowsService.MainWindowOnScene.PlayCloseDoorsModeButton.Button.onClick.AddListener(NextTask);
        }

        protected override void ConfigureNextTask()
        {
            switch (_educationStage)
            {
                case 1:
                    _windowsService.MainWindowOnScene.PlayCloseDoorsModeButton.Button.onClick.RemoveListener(NextTask);

                    _progressService.ObservableProgress.points.Changed += NextTask;
                    break;
                case 2:
                    break;
                case 3:
                    _progressService.ObservableProgress.points.Changed -= NextTask;

                    _windowsService.WindowOpened += SubscribeNextTaskOnWindowOpened;
                    break;
                case 4:
                    _mergeController.MergeResult += NextTask;

                    List<Coin> coinsToMerge = FindCoinsToMerge();

                    _fingerView = _UIFactory.CreateFingerView();
                    SortFingerViewUnderEducationWindow();

                    _fingerView.OfferingDrag((RectTransform)coinsToMerge[0].transform, (RectTransform)coinsToMerge[1].transform);
                    break;
                case 5:
                    _mergeController.MergeResult -= NextTask;

                    _windowsService.MainWindowOnScene.ScoreCoinsView.SwitchShopButton.onClick.AddListener(NextTask);

                    _fingerView.ResetView();
                    _fingerView.OfferingClick((RectTransform)_windowsService.MainWindowOnScene.ScoreCoinsView.SwitchShopButton.transform);
                    break;
                case 6:
                    _windowsService.MainWindowOnScene.ScoreCoinsView.SwitchShopButton.onClick.RemoveListener(NextTask);

                    _windowsService.Close(WindowId.Education);

                    _timeService.Resume();

                    Object.Destroy(_fingerView.gameObject);

                    _progressService.ObservableProgress.educationCompleted.Value = true;

                    EducationCompleted();
                    break;
            }
        }

        private void SortFingerViewUnderEducationWindow() => 
            _fingerView.transform.SetSiblingIndex(_fingerView.transform.GetSiblingIndex() - 1);

        private void SubscribeNextTaskOnWindowOpened(Window window)
        {
            if (window is ResultCloseDoorGameWindow && _educationStage == 3)
            {
                _windowsService.ResultCloseDoorGameWindowOnScene.ÑontinueButton.Button.onClick.AddListener(NextTask);
                _windowsService.WindowOpened -= SubscribeNextTaskOnWindowOpened;
            }
        }
        private List<Coin> FindCoinsToMerge()
        {
            Inventory inventory = _windowsService.MainWindowOnScene.Inventory;
            List<Coin> result = new List<Coin>();
            for (int i = 0; i < inventory.ItemsParent.transform.childCount - 1; i++)
            {
                for (int j = i + 1; j < inventory.ItemsParent.transform.childCount; j++)
                {
                    if (i == j) continue;

                    if (inventory.ItemsParent.transform.GetChild(i).GetComponent<Coin>().Upgrade ==
                        inventory.ItemsParent.transform.GetChild(j).GetComponent<Coin>().Upgrade)
                    {
                        result.Add(inventory.ItemsParent.transform.GetChild(i).GetComponent<Coin>());
                        result.Add(inventory.ItemsParent.transform.GetChild(j).GetComponent<Coin>());
                        return result;
                    }
                }
            }
            result.Add(inventory.CreateBaseCoin());
            result.Add(inventory.CreateBaseCoin());
            return result;
        }
        private void NextTask(int _) =>
            NextTask();
        private void NextTask(Vector3 _, string __, Color ___) =>
            NextTask();
    }
}