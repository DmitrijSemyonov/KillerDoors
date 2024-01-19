using KillerDoors.Common;
using KillerDoors.Services.Merge;
using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.ScoreSpace;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.StaticDataSpace;
using KillerDoors.UI.Factories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KillerDoors.UI.SpotAppearanceSpace
{
    public class SpotAppearanceViewService : ISpotAppearanceViewService
    {
        private readonly IUIFactory _UIFactory;
        private readonly IMergeService _mergeService;
        private readonly IScoreService _scoreService;
        private readonly IMonoBehaviourService _monoBehaviourService;
        private readonly IStaticDataService _staticDataService;

        private Camera _mainCamera;
        private LosingZone _losingZone;

        private PoolSpotAppearanceView _pool;

        private GameStaticData.SpotAppearanceView _spotAppearanceViewData;
        public SpotAppearanceViewService(IUIFactory UIFactory, IMergeService mergeService, IScoreService scoreService, IMonoBehaviourService monoBehaviourService,
            IStaticDataService staticDataService)
        {
            _UIFactory = UIFactory;
            _mergeService = mergeService;
            _scoreService = scoreService;
            _monoBehaviourService = monoBehaviourService;
            _staticDataService = staticDataService;

            _scoreService.ComboChangeAt += ShowEarningPoints;
            _mergeService.MergeResult += ShowMergeResult;
        }
        public void Init(LosingZone losingZone)
        {
            _losingZone = losingZone;
            _mainCamera = Camera.main;
            _spotAppearanceViewData = _staticDataService.GetGameData().spotAppearanceViewData;
            _pool = new PoolSpotAppearanceView(_UIFactory);

            SubscribeSceneObjects();
        }
        private void SubscribeSceneObjects()
        {
            _losingZone.LosePerson += ShowZeroingCombo;
        }
        private void ShowEarningPoints(Vector3 worldPosition, int points)
        {
            SpotAppearanceView spotAppearanceView = _pool.GetView();

            ((RectTransform)spotAppearanceView.transform).position = _mainCamera.WorldToScreenPoint(worldPosition + _spotAppearanceViewData.offsetEaningPointsWorld);

            spotAppearanceView.LocalizedText.Localize("Combo +", subKeys: new List<string>() { points.ToString() }  );

            spotAppearanceView.Text.color = Color.yellow;

            ClampPositionInCameraView(spotAppearanceView);

            spotAppearanceView.AppearanceDisappearance.StartAppearing();

            _monoBehaviourService.StartCoroutine(DelayedDisappearing(spotAppearanceView));
        }
        private void ShowZeroingCombo(Vector3 worldPosition)
        {
            SpotAppearanceView spotAppearanceView = _pool.GetView();

            ((RectTransform)spotAppearanceView.transform).position = _mainCamera.WorldToScreenPoint(worldPosition + _spotAppearanceViewData.offsetEaningPointsWorld);

            spotAppearanceView.LocalizedText.Localize("Combo", subKeys: new List<string>() { 0.ToString() });

            spotAppearanceView.Text.color = Color.red;

            ClampPositionInCameraView(spotAppearanceView);

            spotAppearanceView.AppearanceDisappearance.StartAppearing();

            _monoBehaviourService.StartCoroutine(DelayedDisappearing(spotAppearanceView));
        }
        private void ShowMergeResult(Vector3 cameraPosition, string text, Color color)
        {
            SpotAppearanceView spotAppearanceView = _pool.GetView();

            ((RectTransform)spotAppearanceView.transform).position = cameraPosition + _spotAppearanceViewData.offsetMergeResult;

            spotAppearanceView.LocalizedText.Localize(text);

            spotAppearanceView.Text.color = color;

            ClampPositionInCameraView(spotAppearanceView);

            spotAppearanceView.AppearanceDisappearance.StartAppearing();

            _monoBehaviourService.StartCoroutine(DelayedDisappearing(spotAppearanceView));
        }
        private IEnumerator DelayedDisappearing(SpotAppearanceView spotAppearanceView)
        {
            yield return new WaitForSecondsRealtime(_spotAppearanceViewData.retentionView);
            spotAppearanceView.AppearanceDisappearance.StartDisappearing();
            yield return new WaitForSecondsRealtime(0.3f);
            _pool.Release(spotAppearanceView);
        }
        private static void ClampPositionInCameraView(SpotAppearanceView spotAppearanceView)
        {
            RectTransform rect = (RectTransform)spotAppearanceView.transform;

            if (rect.position.x + spotAppearanceView.Text.preferredWidth / 2f > Screen.width)
                rect.position = new Vector2(Screen.width - spotAppearanceView.Text.preferredWidth / 2f - 80f, rect.position.y);
            else if (rect.position.x - spotAppearanceView.Text.preferredWidth / 2f < 80f)
                rect.position = new Vector2(spotAppearanceView.Text.preferredWidth / 2f + 80f, rect.position.y);
        }
    }
}