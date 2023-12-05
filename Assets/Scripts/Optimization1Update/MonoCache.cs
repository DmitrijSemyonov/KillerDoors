using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoCache : MonoBehaviour
{
    public static List<MonoCache> allUpdate = new List<MonoCache>(400);

    protected virtual void OnEnable() => allUpdate.Add(this);
    protected virtual void OnDisable() => allUpdate.Remove(this);
    protected virtual void OnDestroy() => allUpdate.Remove(this);

    public void Tick() => OnTick();
    protected virtual void OnTick() { }
}
