using System.Collections;
using System.IO;
using UnityEngine;

namespace Helpers.Screenshot
{
    public class ScreenshotMaker : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private KeyCode _screenButton = KeyCode.Z;

        [SerializeField] private bool _interfaceControl;

        private const string ScreenshotsPath = "Screenshots";

        private void Update()
        {
            if (Input.GetKeyDown(_screenButton))
                StartCoroutine(TakeScreenshot());
        }
        public IEnumerator TakeScreenshot()
        {
            CutElement[] cutElements = null;

            if (_interfaceControl)
            {
                cutElements = FindObjectsOfType<CutElement>();

                for (int j = 0; j < cutElements.Length; j++)
                    cutElements[j].gameObject.SetActive(false);

                yield return new WaitForEndOfFrame();
            }

            if (!Directory.Exists(ScreenshotsPath))
                Directory.CreateDirectory(ScreenshotsPath);

            int i = 0;
            while (File.Exists(ScreenshotsPath + "/" + i + ".png")) i++;

            ScreenCapture.CaptureScreenshot(ScreenshotsPath + "/" + i + ".png");

            if (_interfaceControl)
            {
                for (int j = 0; j < cutElements.Length; j++)
                    cutElements[j].gameObject.SetActive(true);
            }
        }
#endif
    }
}