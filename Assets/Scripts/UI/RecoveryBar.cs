using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecoveryBar : MonoBehaviour
{
    [SerializeField] private Image _progressIm;
    [SerializeField] private Color _progressColor;
    [SerializeField] private Color _completeColor;
    private void Start()
    {
        GetComponentInParent<Door>().OpenProgress += RecoveryUpdate;
        if (!_progressIm) _progressIm = GetComponent<Image>();
    }
    public void RecoveryUpdate(float percent)
    {
        _progressIm.fillAmount = percent;
        if(percent < 1f)
        {
            _progressIm.color = _progressColor;
        }
        else
        {
            _progressIm.color = _completeColor;
        }
    }
}
