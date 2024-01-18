using KillerDoors.Services.MonoBehaviourFunctions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KillerDoors.Services.SceneLoad
{
    public class SceneLoader : ISceneLoadService
    {
        private IMonoBehaviourService _coroutineRunner;

        public SceneLoader(IMonoBehaviourService coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        public void SceneLoad(string sceneName, Action onCompleteLoad = null) =>
            _coroutineRunner.StartCoroutine(Load(sceneName, onCompleteLoad));
        private IEnumerator Load(string sceneName, Action onCompleteLoad = null)
        {
            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!sceneLoad.isDone)
                yield return null;

            onCompleteLoad?.Invoke();
        }
    }
}














/*
public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
{
    var tcs = new TaskCompletionSource<AsyncOperation>();
    asyncOp.completed += operation => { tcs.SetResult(operation); };
    return ((Task)tcs.Task).GetAwaiter();
}
*/
