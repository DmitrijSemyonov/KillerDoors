using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDoorView : MonoCache
{
    private Camera _mainCamera;
    [SerializeField] private DoorUpgrade _doorUpgrade;
    [SerializeField] private TextMeshProUGUI _nextUpgradePriceText;
    private AppearingAndDisappearingObject _nextUpgradePriceAppearing;
    private Button _button;
    [SerializeField] private Vector3 _offsetPosition;
    void Start()
    {
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(_doorUpgrade.UpgradeDoor);

        _nextUpgradePriceAppearing = _nextUpgradePriceText.GetComponent<AppearingAndDisappearingObject>();
        _mainCamera = Camera.main;

        _doorUpgrade.UpgradePriceChanged += UpdateNextUpgradePrice;
        enabled = false;
    }
    protected override void OnTick()
    {
       UpdateViewPosition();
    }
    public void UpdateViewPosition()
    {
        ((RectTransform)transform).position = _mainCamera.WorldToScreenPoint(_doorUpgrade.transform.position) + _offsetPosition;
    }
    private void UpdateNextUpgradePrice(int price)
    {
        _nextUpgradePriceText.text = price.ToString();
        _nextUpgradePriceText.transform.localScale = CachedMath.Vector3Zero;
        _nextUpgradePriceAppearing.StartAppearing();
    }
    protected override void OnDestroy()
    {
        if(_doorUpgrade)
          _doorUpgrade.UpgradePriceChanged -= UpdateNextUpgradePrice;
        base.OnDestroy();

    }
}
