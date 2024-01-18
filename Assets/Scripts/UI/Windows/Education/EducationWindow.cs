using UnityEngine;
using UnityEngine.UI;
using KillerDoors.Services.Education;
using KillerDoors.Services.Localization;

namespace KillerDoors.UI.Windows
{
    public class EducationWindow : Window
    {
        [field: SerializeField] public Button ContinueButton { get; private set; }

        private IEducationService _educationService;

        [SerializeField] private LocalizedText _educationText;

        public void Construct(IEducationService educationService)
        {
            _educationService = educationService;
            ContinueButton.onClick.AddListener(Continue);
        }
        public void SetStage(int stage)
        {
            _educationText.Localize("LT" + stage.ToString());
            AppearanceDisappearance.StartAppearing();
        }
        private void Continue() =>
            _educationService.ContinueAfterReading();
    }
}