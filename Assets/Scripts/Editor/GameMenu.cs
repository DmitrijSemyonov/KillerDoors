using KillerDoors.Services.Localization;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace EditorSpace
{
    public class GameMenu : EditorWindow
    {
        [MenuItem("Game/Clear player prefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Game/Take screenshot")]
        public static void TakeScreenshot()
        {
            string path = "Screenshots";

            Directory.CreateDirectory(path);

            int i = 0;
            while (File.Exists(path + "/" + i + ".png")) i++;

            ScreenCapture.CaptureScreenshot(path + "/" + i + ".png");
        }

        [MenuItem("Game/Add localized texts to project")]
        public static void AddLocalizedTextsToProject()
        {
            var texts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                if (!text.GetComponent<LocalizedText>() &&
                    !text.GetComponentInParent<TMP_Dropdown>() &&
                    !text.GetComponent<IgnoreLocalize>() &&
                    !string.IsNullOrEmpty(text.text))
                {
                    text.gameObject.AddComponent<LocalizedText>();
                }
            }
        }
        [MenuItem("Game/Add localized texts to scene")]
        public static void AddLocalizedTextsToScene()
        {
            var texts = FindObjectsOfType<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                if (!text.GetComponent<LocalizedText>() &&
                    !text.GetComponentInParent<TMP_Dropdown>() &&
                    !text.GetComponent<IgnoreLocalize>() &&
                    !string.IsNullOrEmpty(text.text))
                {
                    text.gameObject.AddComponent<LocalizedText>();
                }
            }
        }
    }
}