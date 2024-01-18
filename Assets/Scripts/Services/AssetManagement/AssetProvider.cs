using UnityEngine;

namespace KillerDoors.Services.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public T Instantiate<T>(string path) where T : Object =>
           Object.Instantiate(Load<T>(path));

        public T Instantiate<T>(string path, Vector3 at) where T : Object =>
            Object.Instantiate(Load<T>(path), at, Quaternion.identity);

        public T Instantiate<T>(string path, Vector3 at, Vector3 rotation) where T : Object =>
            Object.Instantiate(Load<T>(path), at, Quaternion.Euler(rotation));
        public T Instantiate<T>(string path, Vector3 at, Quaternion rotation, Transform parent) where T : Object =>
            Object.Instantiate(Load<T>(path), at, rotation, parent);

        public T Instantiate<T>(string path, Transform parent) where T : Object =>
            Object.Instantiate(Load<T>(path), parent);

        public T Load<T>(string path) where T : Object =>
           Resources.Load<T>(path);
    }
}