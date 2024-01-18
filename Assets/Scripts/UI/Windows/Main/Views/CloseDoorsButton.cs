using KillerDoors.Services.DoorControl;
using UnityEngine;
using UnityEngine.UI;

namespace KillerDoors.UI.Windows.Main.Views
{
    public class CloseDoorsButton : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        private IDoorController _doorController;

        public void Construct(IDoorController doorController)
        {
            _doorController = doorController;
            Button.onClick.AddListener(TryCloseDoors);
        }
        public void TryCloseDoors() =>
            _doorController.TryCloseAllDoors();
    }
}