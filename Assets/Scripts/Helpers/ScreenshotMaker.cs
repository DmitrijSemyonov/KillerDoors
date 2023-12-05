
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotMaker : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private KeyCode _screenButton = KeyCode.Z;
    [SerializeField] bool _interfaceControl;
    [Header("All arrays must be the same length")]
    [SerializeField] private bool[] _disableInterfaceForScreenshots;
    [Header("Everything below is not necessary")]
    [SerializeField] private GameObject[] _temporarilyDisabledInterface;
    [Header("Find if List GO (upper) is Empty")]
    [SerializeField] private string[] _temporarilyDisabledNamesOfGOInterface;
    private void Start()
    {
        if(_interfaceControl && _temporarilyDisabledInterface.Length == 0)
        {
            _temporarilyDisabledInterface = new GameObject[_temporarilyDisabledNamesOfGOInterface.Length];

            for(int i =0; i< _temporarilyDisabledNamesOfGOInterface.Length; i++)
            {
                _temporarilyDisabledInterface[i] = GameObject.Find(_temporarilyDisabledNamesOfGOInterface[i]).gameObject;
                Debug.Assert(_temporarilyDisabledInterface[i] != null , "No find GO " + _temporarilyDisabledNamesOfGOInterface[i]);
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(_screenButton))
        {
            StartCoroutine(TakeScreenshot());
        }
    }
    public IEnumerator TakeScreenshot()
    {
        if (_interfaceControl)
        {
            for (int j = 0; j < _temporarilyDisabledInterface.Length; j++)
            {
                _temporarilyDisabledInterface[j].SetActive(!_disableInterfaceForScreenshots[j]);
            }
            yield return new WaitForEndOfFrame();
        }
        string path = "Screenshots";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        int i = 0;
        while (File.Exists(path + "/" + i + ".png")) i++;

        ScreenCapture.CaptureScreenshot(path + "/" + i + ".png");

        if (_interfaceControl)
        {
            for (int j = 0; j < _temporarilyDisabledInterface.Length; j++)
            {
                _temporarilyDisabledInterface[j].SetActive(true);
            }
        }
    }
#endif
}