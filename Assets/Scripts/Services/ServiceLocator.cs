namespace KillerDoors.Services
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;

        public static ServiceLocator Container => _instance ?? (_instance = new ServiceLocator());

        internal void RegisterSingle<T>(T implementation) where T : IService =>
            Implementation<T>.ServiceInstance = implementation;

        internal T Single<T>() where T : IService =>
            Implementation<T>.ServiceInstance;
        private static class Implementation<T> where T : IService
        {
            public static T ServiceInstance;
        }
    }
}