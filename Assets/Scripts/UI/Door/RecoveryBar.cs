using KillerDoors.Common;
using UnityEngine;
using UnityEngine.UI;

namespace KillerDoors.UI.DoorUI
{
    public class RecoveryBar : MonoBehaviour
    {
        [SerializeField] private Image _progressIm;
        [SerializeField] private Color _progressColor;
        [SerializeField] private Color _completeColor;
        private Door _door;
        private void Start()
        {
            _door = GetComponentInParent<Door>();
            _door.OpenProgress += RecoveryUpdate;
        }
        public void RecoveryUpdate(float percent)
        {
            _progressIm.fillAmount = percent;

            if (percent < 1f)
                _progressIm.color = _progressColor;
            else
                _progressIm.color = _completeColor;
        }
        private void OnDestroy()
        {
            if (_door)
                _door.OpenProgress -= RecoveryUpdate;
        }
    }
}