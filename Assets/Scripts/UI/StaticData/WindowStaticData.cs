using System.Collections.Generic;
using UnityEngine;

namespace KillerDoors.UI.StaticDataSpace
{
    [CreateAssetMenu(fileName = "WindowStaticData", menuName = "Static Data/Window Static Data")]
    public class WindowStaticData : ScriptableObject
    {
        public List<WindowConfig> configs;
    }
}