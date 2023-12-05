using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DinamitView : MonoBehaviour
{
    private TextMeshProUGUI _dinamitNumberText;
    private AppearingAndDisappearingObject _dinamitNumberAppearing;
    private TextMeshProUGUI _dinamitNextPriceText;
    private AppearingAndDisappearingObject _dinamitNextPriceAppearing;
    private Shop _shop;
    void Awake()
    {
        _shop = FindObjectOfType<Shop>();
        _shop.DinamitCountChanged += UpdateDinamitNumberText;
        _shop.DinamitPriceChanged += UpdateNextDinamitPriceText;

        _dinamitNumberText = GameObject.Find("DinamitNumber Text").GetComponent<TextMeshProUGUI>();
        _dinamitNumberAppearing = _dinamitNumberText.GetComponent<AppearingAndDisappearingObject>();

        _dinamitNextPriceText = GameObject.Find("NextDinamitPrice").GetComponent<TextMeshProUGUI>();
        _dinamitNextPriceAppearing = _dinamitNextPriceText.GetComponent<AppearingAndDisappearingObject>();
    }
    private void UpdateDinamitNumberText(int protection)
    {
        _dinamitNumberText.text = protection.ToString();
        _dinamitNumberAppearing.StartAppearing();
    }
    private void UpdateNextDinamitPriceText(int price)
    {
        _dinamitNextPriceText.text = price.ToString();
        _dinamitNextPriceAppearing.StartAppearing();
    }
    private void OnDestroy()
    {
        if (_shop)
        {
            _shop.DinamitCountChanged -= UpdateDinamitNumberText;
            _shop.DinamitPriceChanged -= UpdateNextDinamitPriceText;
        }
    }
}
