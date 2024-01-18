using Helpers.Math;
using KillerDoors.UI.Factories;
using UnityEngine;
using UnityEngine.Pool;

namespace KillerDoors.UI.SpotAppearanceSpace
{
    public class PoolSpotAppearanceView
    {
        private IObjectPool<SpotAppearanceView> _pool;
        private readonly IUIFactory _UIFactory;

        public bool collectionChecks = true;
        public int maxPoolSize = 10;
        public PoolSpotAppearanceView(IUIFactory uIFactory)
        {
            _UIFactory = uIFactory;
            _pool = new ObjectPool<SpotAppearanceView>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
        }
        public SpotAppearanceView GetView() =>
            _pool.Get();
        public void Release(SpotAppearanceView spotAppearanceView) =>
            _pool.Release(spotAppearanceView);
        private void OnDestroyPoolObject(SpotAppearanceView obj) =>
            Object.Destroy(obj);

        private void OnReturnedToPool(SpotAppearanceView obj) =>
            obj.gameObject.SetActive(false);

        private void OnTakeFromPool(SpotAppearanceView obj) =>
            obj.gameObject.SetActive(true);

        private SpotAppearanceView CreatePooledItem() =>
            _UIFactory.CreateSpotAppearanceView(CachedMath.Vector3Zero);
    }
}