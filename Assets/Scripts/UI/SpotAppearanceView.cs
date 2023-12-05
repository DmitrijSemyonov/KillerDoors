using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpotAppearanceView : MonoBehaviour
{
    private Transform _viewsParent;
    [SerializeField] private AppearingAndDisappearingObject _textPrefab;
    [SerializeField] private Vector3 _offsetEaningPointsWorld = new Vector3(-7f, 15f, 0f);
    [SerializeField] private Vector3 _offsetMergeResult = new Vector3(-40f, 100f, 0f);
    private Camera _mainCamera;
    private ScoreAccounting _scoreAccounting;
    private MergeController _mergeController;
    private LosingZone _losingZone;
    private List<AppearingAndDisappearingObject> _views = new List<AppearingAndDisappearingObject>();
    void Start()
    {
        _mainCamera = Camera.main;
        _viewsParent = GameObject.Find("ParentAppearanceView").transform;
        _scoreAccounting = GetComponent<ScoreAccounting>();
        _scoreAccounting.OnComboChanged += ShowEarningPoints;
        _mergeController = GetComponent<MergeController>();
        _mergeController.MergeResult += ShowMergeResult;
        _losingZone = FindObjectOfType<LosingZone>();
        _losingZone.LosePerson += ShowZeroingCombo;
    }

    private void ShowEarningPoints(Vector3 worldPosition, int points)
    {
        ShowText(_mainCamera.WorldToScreenPoint(worldPosition + _offsetEaningPointsWorld), "+ " + points, Color.yellow);
        ShowText(_mainCamera.WorldToScreenPoint(worldPosition + _offsetEaningPointsWorld + new Vector3(0f, 5f, 0f)), "Combo", Color.yellow);
    }
    private void ShowZeroingCombo(Vector3 worldPosition)
    {
        ShowText(_mainCamera.WorldToScreenPoint(worldPosition + _offsetEaningPointsWorld), "0", Color.red);
        ShowText(_mainCamera.WorldToScreenPoint(worldPosition + _offsetEaningPointsWorld + new Vector3(0f, 5f, 0f)), "Combo", Color.red);
    }
    private void ShowMergeResult(Vector3 cameraPosition, string text, Color color)
    {
        ShowText(cameraPosition + _offsetMergeResult, text, color);
    }
    private void ShowText(Vector3 cameraPosition, string text, Color color)
    {
        AppearingAndDisappearingObject AppearingObj = Instantiate(_textPrefab, cameraPosition, Quaternion.identity, _viewsParent.transform);
        
        TextMeshProUGUI TMPRO = AppearingObj.GetComponent<TextMeshProUGUI>();
        TMPRO.text = text;
        TMPRO.GetComponent<LocalizedText>().Localize(text);
        TMPRO.color = color;

        ClampPositionInCameraView(AppearingObj, TMPRO);

        AppearingObj.StartAppearing();
        _views.Add(AppearingObj);

        ControlViewCount(2);
        Destroy(AppearingObj.gameObject, 1f);
    }

    private static void ClampPositionInCameraView(AppearingAndDisappearingObject AppearingObj, TextMeshProUGUI TMPRO)
    {
        RectTransform rect = (RectTransform)AppearingObj.transform;

        if (rect.position.x + TMPRO.preferredWidth / 2f > Screen.width)
        {
            rect.position = new Vector2(Screen.width - TMPRO.preferredWidth / 2f - 80f, rect.position.y);
        }
        else if (rect.position.x - TMPRO.preferredWidth / 2f < 80f)
        {
            rect.position = new Vector2(TMPRO.preferredWidth / 2f + 80f, rect.position.y);
        }
    }

    private void ControlViewCount(int maxCount)
    {
        int count = _views.Count;
        while (count > maxCount)
        {
            count--;
            if (_views[0] != null)
            {
                _views[0].StartDisappearing();
            }
            _views.RemoveAt(0);
        }
    }

    private void OnDestroy()
    {
        if (_scoreAccounting)
        {
            _scoreAccounting.OnComboChanged -= ShowEarningPoints;
        }
        if (_mergeController)
        {
            _mergeController.MergeResult -= ShowMergeResult;
        }
        if (_losingZone)
        {
            _losingZone.LosePerson -= ShowZeroingCombo;
        }
    }
}
