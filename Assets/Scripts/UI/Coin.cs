using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Coin : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public int Upgrade { get { return _upgrade; } set { _upgrade = value; _upgradeText.text = value.ToString(); } }
    private int _upgrade = 1;
    public int IndexPriviousDragging { get; private set; } = -1;
    [SerializeField] private Image _coinImage;

    private Transform _parent;
    private Transform _parentOnDragging;
    [field:SerializeField] public Transform BackgroundCell { get; private set; }
    private RectTransform _backgroundRect;
    private PlayerDataManager _dataManager;
    private MergeController _mergeController;

    private static int _draggingCount;

    [SerializeField] private TextMeshProUGUI _upgradeText;
    [field: SerializeField] public AppearingAndDisappearingObject AppearingDisappearing { get; private set; }
    [SerializeField] private Animator _animator;
    private int _fastRotateHash = Animator.StringToHash("fastRotate");
    void Start()
    {
        AppearingDisappearing.StartAppearing();
        _backgroundRect = (RectTransform) BackgroundCell;
    }
    public void Init(PlayerDataManager dataManager, Transform parent, Transform parentOnDragging, MergeController mergeController)
    {
        _dataManager = dataManager;
        _parent = parent;
        _parentOnDragging = parentOnDragging;
        _mergeController = mergeController;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_draggingCount != 1) return;

        RectTransform rect = (RectTransform)transform;
        rect.position = eventData.position;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_draggingCount != 0) return;
        _draggingCount++;

        IndexPriviousDragging = transform.GetSiblingIndex();
        BackgroundCell.parent = _parent;
        BackgroundCell.SetSiblingIndex(transform.GetSiblingIndex() + 1);
        transform.parent = _parentOnDragging;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (transform.parent == _parent) return;

        _draggingCount = 0;
        int closestIndex = FindClosestCellIndex();

        if (_parent.GetChild(closestIndex) != BackgroundCell &&
            _mergeController.MergeOperation(transform, _parent.GetChild(closestIndex)))
        {
            return;
        }

        BackgroundCell.parent = transform;
        BackgroundCell.SetSiblingIndex(0);
        _backgroundRect.anchoredPosition = new Vector2(_backgroundRect.rect.width / 2f, -_backgroundRect.rect.height / 2f);

        transform.parent = _parent;
        transform.SetSiblingIndex(closestIndex);
        _dataManager.RemoveDataCoinAfterDragging(IndexPriviousDragging);
        IndexPriviousDragging = -1;

        _dataManager.AddDataCoin(this);
    }

    private int FindClosestCellIndex()
    {
        int closestIndex = 0;
        for (int i = 0; i < _parent.childCount; i++)
        {
            if (Vector2.Distance(transform.position, _parent.GetChild(i).position) <
               Vector2.Distance(transform.position, _parent.GetChild(closestIndex).position))
            {
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    public void PerfomanceMergeResult(bool isSuccessResult)
    {
        Color color;

        if (isSuccessResult)
        {
            Upgrade *= 2;
            transform.localScale = CachedMath.Vector3Zero;
            AppearingDisappearing.StartAppearing();
            color = Color.green;
        }
        else
        {
            color = Color.red;
            Destroy(gameObject, 0.5f);
            _upgrade = -1;
            GetComponent<Image>().raycastTarget = false;
        }

        bool isDraggingThis = transform.parent == _parentOnDragging;
        if (isDraggingThis)
        {
            _upgrade = -1;
            AppearingDisappearing.StartDisappearing();
        }
        else
        {
            _animator.SetTrigger(_fastRotateHash);
        }

        bool destroyAtTheEnd = !isSuccessResult || transform.parent == _parentOnDragging;
        StartCoroutine(ColorBlinkAndMayBeDestroy(color, destroyAtTheEnd));
    }
    private IEnumerator ColorBlinkAndMayBeDestroy(Color color, bool destroyAtTheEnd)
    {
        for (int i = 0; i < 4; i++)
        {
            _coinImage.color = color;
            yield return new WaitForSecondsRealtime(0.06f);
            _coinImage.color = Color.white;
            yield return new WaitForSecondsRealtime(0.06f);
        }
        if (destroyAtTheEnd)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if(BackgroundCell) Destroy(BackgroundCell.gameObject);
    }
}
