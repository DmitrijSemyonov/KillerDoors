using Helpers;
using KillerDoors.Services.Localization;
using TMPro;
using UnityEngine;

namespace KillerDoors.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI TextLoading { get; private set; }
        [field: SerializeField] public AppearingAndDisappearingObject AppearanceDisappearance { get; private set; }

        public void BootstrapInit()
        {
            SaveLocalizeKey();

            if (Application.systemLanguage == SystemLanguage.Russian)
                TextLoading.text = "Загрузка...";
            else
                TextLoading.text = "Loading...";

            AppearanceDisappearance.Disappeared += OnDisappeared;
        }

        private void SaveLocalizeKey()
        {
            TextLoading.GetComponent<LocalizedText>().Init();
        }

        public void ApplyLanguagePreviousLocalization(string language)
        {
            if (TextLoading.GetComponent<LocalizedText>().IsInitialized) return;

            SaveLocalizeKey();

            if (language.Equals("ru"))
                TextLoading.text = "Загрузка...";
            else
                TextLoading.text = "Loading...";
        }
        private void OnDisappeared() =>
            gameObject.SetActive(false);
        private void OnDestroy() =>
            AppearanceDisappearance.Disappeared -= OnDisappeared;
    }
}