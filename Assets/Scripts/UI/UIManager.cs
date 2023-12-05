using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private AppearingAndDisappearingObject _buttonStartCloseDoorsGame;
    private GameObject _buttonCloseDoors;
    private Button _shopButton;
    private AppearingAndDisappearingObject _gameResult;
    private AppearingAndDisappearingObject _shop;
    private bool _isShopActive;
    private Button _exploseDinamitButton;

    private AppearingAndDisappearingObject _logInButton;
    private AppearingAndDisappearingObject[] _adsButtonsOnMainScreen = new AppearingAndDisappearingObject[3];
    private UpgradeDoorView[] _upgradeDoorViews;
    private void Start()
    {
        _upgradeDoorViews = FindObjectsOfType<UpgradeDoorView>();
        _gameResult = GameObject.Find("ResultView").GetComponent<AppearingAndDisappearingObject>();
        _buttonStartCloseDoorsGame = GameObject.Find("ButtonPlay").GetComponent<AppearingAndDisappearingObject>();
        _buttonCloseDoors = GameObject.Find("ButtonCloseDoors");
        _shopButton = GameObject.Find("Button Shop").GetComponent<Button>();
        _shop = GameObject.Find("ShopPanel").GetComponent<AppearingAndDisappearingObject>();
        _buttonCloseDoors.gameObject.SetActive(false);
        _exploseDinamitButton = GameObject.Find("ExploseButton").GetComponent<Button>();
        _exploseDinamitButton.interactable = false;
        _adsButtonsOnMainScreen[0] = GameObject.Find("AdsButtonSlowDownTime").GetComponent<AppearingAndDisappearingObject>();
        _adsButtonsOnMainScreen[1] = GameObject.Find("AdsProtectionButton").GetComponent<AppearingAndDisappearingObject>();
        _adsButtonsOnMainScreen[2] = GameObject.Find("AdsDinamitButton").GetComponent<AppearingAndDisappearingObject>();
        GameObject logObj;
        if (logObj = GameObject.Find("Log&SaveInCloudButton")) 
        {
            _logInButton = logObj.GetComponent<AppearingAndDisappearingObject>();
        }
    }

    public void SetCloseDoorState()
    {
        if (_logInButton)
        {
            _logInButton.StartDisappearing();
        }
        _buttonStartCloseDoorsGame.StartDisappearing();
        _buttonCloseDoors.SetActive(true);
        _shopButton.interactable = false;
        _shop.StartDisappearing();
        _exploseDinamitButton.interactable = true;
        for(int i =0; i < _adsButtonsOnMainScreen.Length; i++)
        {
            _adsButtonsOnMainScreen[i].StartDisappearing();
        }
    }
    public void SetCoinMergeState()
    {
        if (_logInButton)
        {
            _logInButton.StartAppearing();
        }
        _gameResult.StartAppearing();
        _buttonStartCloseDoorsGame.StartAppearing();
        _buttonCloseDoors.SetActive(false);
        _shopButton.interactable = true;
        _exploseDinamitButton.interactable = false;
        SetActiveSlowdownTimeButton(true);
        for (int i = 0; i < _adsButtonsOnMainScreen.Length; i++)
        {
            _adsButtonsOnMainScreen[i].StartAppearing();
        }
    }
    public void SwitchShop()
    {
        _isShopActive = !_isShopActive;
        if (_isShopActive)
        {
            _shop.StartAppearing();
            _buttonStartCloseDoorsGame.StartDisappearing();
            for (int i = 0; i < _adsButtonsOnMainScreen.Length; i++)
            {
                _adsButtonsOnMainScreen[i].StartDisappearing();
            }
            for (int i = 0; i < _upgradeDoorViews.Length; i++)
            {
                _upgradeDoorViews[i].enabled = true;
            }
        }
        else
        {
            _shop.StartDisappearing();
            _buttonStartCloseDoorsGame.StartAppearing();
            for (int i = 0; i < _adsButtonsOnMainScreen.Length; i++)
            {
                _adsButtonsOnMainScreen[i].StartAppearing();
            }
            for (int i = 0; i < _upgradeDoorViews.Length; i++)
            {
                _upgradeDoorViews[i].enabled = false;
            }
        }
    }
    public void SetActiveSlowdownTimeButton(bool isActive)
    {
        _adsButtonsOnMainScreen[0].gameObject.SetActive(isActive);
    }
}
