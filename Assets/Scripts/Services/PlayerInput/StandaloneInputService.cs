using UnityEngine;

namespace KillerDoors.Services.InputSpace
{
    public class StandaloneInputService : IInputService
    {
        public bool IsCloseDoorPressed =>
            Input.GetKeyDown(KeyCode.Space);

        public bool IsDinamitPressed =>
            Input.GetKeyDown(KeyCode.D);
    }
}