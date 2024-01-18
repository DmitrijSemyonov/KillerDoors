using System;

namespace KillerDoors.Services.SceneLoad
{
    public interface ISceneLoadService : IService
    {
        void SceneLoad(string sceneName, Action onCompleteLoad = null);
    }
}