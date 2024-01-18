using System;
using System.Collections;
using UnityEngine;

namespace KillerDoors.Services.MonoBehaviourFunctions
{
    public interface IMonoBehaviourService : IService
    {
        event Action<bool> ApplicationFocus;

        void AddUpdateListener(IUpdateListener listener);
        void RemoveUpdateListener(IUpdateListener listener);
        void RemoveAllUpdateListeners();
        Coroutine StartCoroutine(IEnumerator enumerator);
        void StopCoroutine(Coroutine coroutine);
    }
}