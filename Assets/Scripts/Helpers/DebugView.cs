using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugView : MonoBehaviour
{
    public static DebugView instance;
    [SerializeField] private TextMeshProUGUI _debugTextPrefab;
    [SerializeField] private Transform contentParent;
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void Log(string message)
    {
        TextMeshProUGUI debugText = Instantiate(_debugTextPrefab, contentParent);
        debugText.text = message + "\t" + DateTime.Now;
    }
}
