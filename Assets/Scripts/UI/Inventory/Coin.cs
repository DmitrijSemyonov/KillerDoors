using Helpers;
using KillerDoors.Services.Merge;
using KillerDoors.Services.ProgressSpace;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KillerDoors.UI.InventorySpace
{
    public class Coin : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private TextMeshProUGUI _upgradeText;
        [SerializeField] private Image _coinImage;
        [SerializeField] private Animator _animator;
        [field: SerializeField] public Transform BackgroundCell { get; private set; }
        [field: SerializeField] public AppearingAndDisappearingObject AppearingDisappearing { get; private set; }

        private IProgressService _progressService;
        private IMergeService _mergeService;
        private Transform _parent;
        private Transform _parentOnDragging;
        private RectTransform _backgroundRect;
        public int Upgrade
        {
            get { return _upgrade; }
            set
            {
                _upgrade = value;
                _upgradeText.text = value.ToString();
            }
        }
        private int _upgrade = 1;
        public int IndexPriviousDragging { get; private set; } = -1;

        private static int _draggingCount;

        private int _fastRotateHash = Animator.StringToHash("fastRotate");

        public void Init(Transform parent, Transform parentOnDragging, IMergeService mergeService, IProgressService progressService)
        {
            _parent = parent;
            _parentOnDragging = parentOnDragging;
            _mergeService = mergeService;
            _progressService = progressService;

            _backgroundRect = (RectTransform)BackgroundCell;

            AppearingDisappearing.StartAppearing();
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
                _mergeService.MergeOperation(transform, _parent.GetChild(closestIndex)))
            {
                return;
            }

            BackgroundCell.parent = transform;
            BackgroundCell.SetSiblingIndex(0);
            _backgroundRect.anchoredPosition = new Vector2(_backgroundRect.rect.width / 2f, -_backgroundRect.rect.height / 2f);

            transform.parent = _parent;
            transform.SetSiblingIndex(closestIndex);

            _progressService.CoinsDataManager.RemoveDataCoinAfterDragging(IndexPriviousDragging);
            IndexPriviousDragging = -1;

            _progressService.CoinsDataManager.AddDataCoin(this);
        }

        private int FindClosestCellIndex()
        {
            int closestIndex = 0;
            for (int i = 0; i < _parent.childCount; i++)
            {
                if (Vector2.Distance(transform.position, _parent.GetChild(i).position) <
                   Vector2.Distance(transform.position, _parent.GetChild(closestIndex).position))
                    closestIndex = i;
            }
            return closestIndex;
        }

        public void PerfomanceMergeResult(bool isSuccessResult)
        {
            _animator.SetTrigger(_fastRotateHash);

            Color color;

            if (isSuccessResult)
            {
                Upgrade *= 2;
                AppearingDisappearing.StartAppearing();
                color = Color.green;
            }
            else
            {
                color = Color.red;
                _upgrade = -1;
                GetComponent<Image>().raycastTarget = false;
            }
            StartCoroutine(ColorBlinkAndMayBeDestroy(color, !isSuccessResult));
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
                Destroy(gameObject);
        }
        private void OnDestroy()
        {
            if (BackgroundCell) 
                Destroy(BackgroundCell.gameObject);
        }
    }
}