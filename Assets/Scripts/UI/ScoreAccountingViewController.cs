using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreAccountingViewController : MonoBehaviour
{
    private TextMeshProUGUI _pointsCountText;
    private TextMeshProUGUI _coinsCountText;
    private TextMeshProUGUI _coinsResultText;
    private AppearingAndDisappearingObject _pointsCountAnimation;
    private AppearingAndDisappearingObject _coinsCountAnimation;
    private Animator _coinsAnimator;
    private int _fastRotateHash = Animator.StringToHash("fastRotate");
    private ScoreAccounting _scoreAccounting;
    private void Start()
    {
        _pointsCountText = GameObject.Find("PointsCount Text").GetComponent<TextMeshProUGUI>();
        _coinsCountText = GameObject.Find("CoinCounter Text").GetComponent<TextMeshProUGUI>();
        _coinsResultText = GameObject.Find("CoinsEarned text").GetComponent<TextMeshProUGUI>();
        _pointsCountAnimation = _pointsCountText.GetComponent<AppearingAndDisappearingObject>();
        _coinsCountAnimation = _coinsCountText.GetComponent<AppearingAndDisappearingObject>();
        _coinsAnimator = GameObject.Find("Coins View").GetComponentInParent<Animator>();

        _scoreAccounting = GetComponent<ScoreAccounting>();
        _scoreAccounting.OnPointsChanged += UpdatePointsAndCoinsResultTexts;
        _scoreAccounting.OnCoinsChanged += UpdateCoinsText;
    }
    private void UpdatePointsAndCoinsResultTexts(int points)
    {
        _pointsCountText.text = points.ToString();
        _pointsCountAnimation.transform.localScale = CachedMath.Vector3Zero;
        _pointsCountAnimation.StartAppearing();
        if (points != 0)
        {
            _coinsResultText.text = ((int)Math.Log(points, 2)).ToString();
        }
        else
        {
            _pointsCountText.text = "";
        }
    }

    private void UpdateCoinsText(int coins)
    {
        _coinsCountText.text = coins.ToString();
        _coinsCountAnimation.transform.localScale = CachedMath.Vector3Zero;
        _coinsCountAnimation.StartAppearing();
        _coinsAnimator.SetTrigger(_fastRotateHash);
    }
    private void OnDestroy()
    {
        if (_scoreAccounting)
        {
            _scoreAccounting.OnPointsChanged -= UpdatePointsAndCoinsResultTexts;
            _scoreAccounting.OnCoinsChanged -= UpdateCoinsText;
        }
    }
}
