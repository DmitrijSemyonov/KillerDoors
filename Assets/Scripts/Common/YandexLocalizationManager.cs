using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YandexLocalizationManager : LocalizationManager
{
    protected override void GetLanguageAndSetLocalization()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        StartCoroutine(WaitForInitializeYsdkAndSetLanguage());
#endif
#if !UNITY_WEBGL || UNITY_EDITOR
        Debug.Log("Build will be used language from Yandex sdk.");
        base.GetLanguageAndSetLocalization();
#endif
    }
    private IEnumerator WaitForInitializeYsdkAndSetLanguage()
    {
        while (!Yandex.instance.IsYsdkInitialized)
        {
            yield return new WaitForEndOfFrame();
        }
        int idLang = 0;
        string lang = Yandex.instance.Geti18NLanguage();
        if (lang.Equals("ru"))
        {
            idLang = 1;
        }
        else
        {
            idLang = 0;
        }
        SetLanguage(idLang);
        yield break;
    }
}
