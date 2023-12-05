using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoLoadLevel : MonoBehaviour
{
    [Header("Waiting worked only on WebGL")]
    [SerializeField] private bool _waitForYsdkInitialized = true;
    private void Start()
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        _waitForYsdkInitialized = false;
#endif
        if (!_waitForYsdkInitialized)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.Assert(Yandex.instance != null, "Yandex instance is null.");
            StartCoroutine(WaitForYsdkInitialized(1f));
        }
    }
    private IEnumerator WaitForYsdkInitialized(float seconds)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(seconds);
            if(Yandex.instance.IsYsdkInitialized)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
}
