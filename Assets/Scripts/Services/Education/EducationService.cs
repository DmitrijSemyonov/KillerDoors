using KillerDoors.Services.Merge;
using KillerDoors.Services.MonoBehaviourFunctions;
using KillerDoors.Services.ProgressSpace;
using KillerDoors.Services.TimeSpace;
using KillerDoors.UI.Factories;
using KillerDoors.UI.Windows;

namespace KillerDoors.Services.Education
{
    public class EducationService : IEducationService
    {
        private readonly IProgressService _progressService;
        private readonly IUIFactory _UIFactory;
        private readonly IMergeService _mergeController;
        private readonly IWindowsService _windowsService;
        private readonly ITimeService _timeService;
        private readonly IMonoBehaviourService _monoBehaviourService;

        private EducationBase _currentEducation;

        public EducationService(IMergeService mergeService, IProgressService progressService, IWindowsService windowsService, ITimeService timeService,
            IUIFactory UIFactory, IMonoBehaviourService monoBehaviourService)
        {
            _mergeController = mergeService;
            _progressService = progressService;
            _UIFactory = UIFactory;
            _windowsService = windowsService;
            _timeService = timeService;
            _monoBehaviourService = monoBehaviourService;
        }
        public void Init()
        {
            if (!_progressService.ObservableProgress.educationCompleted.Value)
            {
                _currentEducation = new StartEducation(this, _mergeController, _progressService, _windowsService, _timeService, _UIFactory, _monoBehaviourService);
                _currentEducation.Init();
            }
        }
        public void ContinueAfterReading() =>
            _currentEducation.ContinueAfterReading();

        public void OnEducationCompleted()
        {
            _currentEducation = null;
        }
    }
}