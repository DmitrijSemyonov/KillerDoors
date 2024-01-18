using System;
using System.Collections.Generic;
using UnityEngine;

namespace KillerDoors.Services.MonoBehaviourFunctions
{
    public class MonoBehaviourService : MonoBehaviour, IMonoBehaviourService
    {
        private List<IUpdateListener> _allUpdate = new List<IUpdateListener>();
        public event Action<bool> ApplicationFocus;
        void Update()
        {
            for (int i = 0; i < _allUpdate.Count; i++)
            {
                _allUpdate[i].Tick();
            }
        }
        public void AddUpdateListener(IUpdateListener listener) => 
            _allUpdate.Add(listener);
        public void RemoveUpdateListener(IUpdateListener listener) =>
            _allUpdate.Remove(listener);
        public void RemoveAllUpdateListeners() => 
            _allUpdate.Clear();
        private void OnApplicationFocus(bool focus) =>
            ApplicationFocus?.Invoke(focus);
    }
}