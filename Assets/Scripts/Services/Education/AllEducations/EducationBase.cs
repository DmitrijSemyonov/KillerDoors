using UnityEngine;
using System.Collections;
using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.TimeSpace;
using KillerDoors.UI.Windows;
using KillerDoors.UI.StaticDataSpace;

namespace KillerDoors.Services.Education
{
    public abstract class EducationBase
    {
        private readonly IEducationService _educationService;
        protected readonly IWindowsService _windowsService;
        protected readonly ITimeService _timeService;
        private readonly IMonoBehaviourService _monoBehaviourService;

        private Coroutine _delayedNextTask;

        private const float _lockTime = 0.6f;

        private float _lastCompletedTaskTime;

        private bool IsLockTimeElapsed =>
            Time.unscaledTime - _lastCompletedTaskTime > _lockTime;

        protected int _educationStage;

        public EducationBase(IEducationService educationService, IWindowsService windowsService, ITimeService timeService, IMonoBehaviourService monoBehaviourService)
        {
            _educationService = educationService;
            _windowsService = windowsService;
            _timeService = timeService;
            _monoBehaviourService = monoBehaviourService;
        }
        protected abstract void ConfigureNextTask();
        public abstract void Init();

        public void ContinueAfterReading()
        {
            if (!IsLockTimeElapsed) return;

            _windowsService.EducationWindowOnScene.AppearanceDisappearance.StartDisappearing();

            _timeService.Resume();
        }
        protected void NextTask()
        {
            if (_delayedNextTask != null)
                _monoBehaviourService.StopCoroutine(_delayedNextTask);

            _delayedNextTask = _monoBehaviourService.StartCoroutine(DelayedNextTask());
        }
        protected void ShowWindow()
        {
            if (!_windowsService.EducationWindowOnScene)
                _windowsService.Open(WindowId.Education);

            _windowsService.EducationWindowOnScene.SetStage(_educationStage);
        }
        protected void EducationCompleted() =>
            _educationService.OnEducationCompleted();

        private IEnumerator DelayedNextTask()
        {
            yield return new WaitForSeconds(0.7f);

            _educationStage++;

            ShowWindow();

            _timeService.Pause();

            ConfigureNextTask();

            _lastCompletedTaskTime = Time.unscaledTime;

            _delayedNextTask = null;
        }
    }
}