using UnityEngine;
using TMPro;
using Helpers;
using KillerDoors.Services.Localization;

namespace KillerDoors.UI.SpotAppearanceSpace
{
    public class SpotAppearanceView : MonoBehaviour
    {
        [field: SerializeField] public AppearingAndDisappearingObject AppearanceDisappearance { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
        [field: SerializeField] public LocalizedText LocalizedText { get; private set; }

    }
}