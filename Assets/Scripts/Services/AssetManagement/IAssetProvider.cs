using UnityEngine;

namespace KillerDoors.Services.AssetManagement
{
    public interface IAssetProvider : IService
    {
        T Instantiate<T>(string path) where T : Object;
        T Instantiate<T>(string path, Vector3 at) where T : Object;
        T Instantiate<T>(string path, Transform parent) where T : Object;
        T Instantiate<T>(string path, Vector3 at, Vector3 rotation) where T : Object;
        T Instantiate<T>(string path, Vector3 at, Quaternion rotation, Transform parent) where T : Object;
        T Load<T>(string icon) where T : Object;
    }
}