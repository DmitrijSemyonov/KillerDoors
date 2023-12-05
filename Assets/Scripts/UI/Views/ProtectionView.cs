using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProtectionView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _protectionNumberText;
    private AppearingAndDisappearingObject _appearingAndDisappearing;
    private LosingZone _losingZone;
    private Shop _shop;

    private TextMeshProUGUI _textNextProtectionPrice;

    void Start()
    {
        _appearingAndDisappearing = _protectionNumberText.GetComponent<AppearingAndDisappearingObject>();
        _losingZone = FindObjectOfType<LosingZone>();
        _losingZone.ProtectionChanged += UpdateProtectionNumberText;

        _textNextProtectionPrice = GameObject.Find("NextProtectionPrice").GetComponent<TextMeshProUGUI>();
        _shop = FindObjectOfType<Shop>();
        _shop.ProtectionPriceChanged += UpdateNextProtectionPrice;
    }

    private void UpdateProtectionNumberText(int protection)
    {
        _protectionNumberText.transform.localScale = CachedMath.Vector3Zero;
        _protectionNumberText.text = protection.ToString();
        _appearingAndDisappearing.StartAppearing();
    }
    private void UpdateNextProtectionPrice(int price)
    {
        _textNextProtectionPrice.text = price.ToString();
    }
    private void OnDestroy()
    {
        if(_losingZone)
            _losingZone.ProtectionChanged -= UpdateProtectionNumberText;
        if(_shop)
            _shop.ProtectionPriceChanged += UpdateNextProtectionPrice;
    }
}
