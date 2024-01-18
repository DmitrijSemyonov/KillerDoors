using UnityEngine;
using Helpers;

namespace KillerDoors.UI.Windows
{
    public abstract class Window : MonoBehaviour
    {
        [field: SerializeField] public AppearingAndDisappearingObject AppearanceDisappearance { get; private set; }

        protected virtual void Start() => 
            AppearanceDisappearance.StartAppearing();
        public void Close()
        {
            AppearanceDisappearance.StartDisappearing();
            Destroy(gameObject, 0.5f);
        }
    }
}