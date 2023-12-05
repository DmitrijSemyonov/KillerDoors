using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Education : MonoBehaviour
{
    private const float _lockTime = 0.6f;
    private Button _shopButton;
    private Button _playButton;
    private Button _resultButton;
    private FingerAnimation _fingerAnimation;
    private TimeController _timeController;
    private ScoreAccounting _scoreAccounting;
    private MergeController _mergeController;
    private AppearingAndDisappearingObject _appearingDisappearing;
    private LocalizedText _educationText;
    private int _educationStage;
    private float _lastCompletedTaskTime;
    void Start()
    {
        if (PlayerDataLoader.Instance.playerData.educationCompleted)
        {
            Destroy(gameObject);
            return;
        }
        _educationText = GameObject.Find("EducationText").GetComponent<LocalizedText>();
        _appearingDisappearing = GetComponent<AppearingAndDisappearingObject>();
        _scoreAccounting = FindObjectOfType<ScoreAccounting>();
        _mergeController = _scoreAccounting.GetComponent<MergeController>();
        _timeController = FindObjectOfType<TimeController>();
        _fingerAnimation = FindObjectOfType<FingerAnimation>();
        _shopButton = GameObject.Find("Button Shop").GetComponent<Button>();
        _playButton = GameObject.Find("ButtonPlay").GetComponent<Button>();
        _resultButton = GameObject.Find("ResultView").GetComponent<Button>(); 
        _playButton.onClick.AddListener(NextLesson);
    }
    public void ShowPanel()
    {
        _appearingDisappearing.StartAppearing();
        _timeController.Pause();
    }
    public void Continue()
    {
        bool isLocked = Time.unscaledTime - _lastCompletedTaskTime < _lockTime;
        if (isLocked) return;

        _appearingDisappearing.StartDisappearing();
        _timeController.Resume();
    }
    public void NextLesson()
    {
        _educationStage++;
        _educationText.Localize("LT" + _educationStage.ToString());
        ShowPanel();
        ConfigureNextTask();
        _lastCompletedTaskTime = Time.unscaledTime;
    }
    private void NextLesson(int _)
    {
        NextLesson();
    }
    private void ConfigureNextTask()
    {
        switch (_educationStage)
        {
            case 1:
                _playButton.onClick.RemoveListener(NextLesson);
                _scoreAccounting.OnPointsChanged += NextLesson;
                break;
            case 2:
                break;
            case 3:
                _scoreAccounting.OnPointsChanged -= NextLesson;
                _resultButton.onClick.AddListener(NextLesson);
                break;
            case 4:
                _resultButton.onClick.RemoveListener(NextLesson);
                _mergeController.MergeResult += NextLesson;
                List<Coin> coinsToMerge = FindCoinsToMerge();
                _fingerAnimation.OfferingDrag((RectTransform)coinsToMerge[0].transform, (RectTransform)coinsToMerge[1].transform);
                break;
            case 5:
                _mergeController.MergeResult -= NextLesson;
                _shopButton.onClick.AddListener(NextLesson);
                _fingerAnimation.ResetAnimation();
                _fingerAnimation.OfferingClick((RectTransform)_shopButton.transform);
                break;
            case 6:
                _shopButton.onClick.RemoveListener(NextLesson);
                PlayerDataLoader.Instance.playerData.educationCompleted = true;
                PlayerDataLoader.Instance.SaveData();
                _timeController.Resume();
                Destroy(_fingerAnimation.gameObject);
                Destroy(gameObject);
                break;
        }
    }
    private List<Coin> FindCoinsToMerge()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        List<Coin> result = new List<Coin>();
        for(int i =0; i < inventory.transform.childCount - 1; i++)
        {
            for (int j = i + 1; j < inventory.transform.childCount; j++)
            {
                if (i == j) continue;

                if (inventory.transform.GetChild(i).GetComponent<Coin>().Upgrade ==
                    inventory.transform.GetChild(j).GetComponent<Coin>().Upgrade)
                {
                    result.Add(inventory.transform.GetChild(i).GetComponent<Coin>());
                    result.Add(inventory.transform.GetChild(j).GetComponent<Coin>());
                    return result;
                }
            }
        }
        result.Add(inventory.CreateCoin());
        result.Add(inventory.CreateCoin());
        return result;
    }
    private void NextLesson(Vector3 _, string __, Color ___)
    {
        NextLesson();
    }
    private void OnDestroy()
    {
        if(_playButton)
            _playButton.onClick.RemoveListener(NextLesson);
        if (_shopButton)
             _shopButton.onClick.RemoveListener(NextLesson);
        if(_mergeController)
            _mergeController.MergeResult -= NextLesson;
        if(_scoreAccounting)
            _scoreAccounting.OnPointsChanged -= NextLesson;
        if (_resultButton)
            _resultButton.onClick.RemoveListener(NextLesson);
    }
}
