using KillerDoors.InitGame;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KillerDoors
{
    public class RunFromAnyScene : MonoBehaviour
    {
        private const string StartSceneName = "StartScene";

        private void Awake()
        {
#if UNITY_EDITOR
            GameBootstrap gameBootstrap = FindObjectOfType<GameBootstrap>();
            if (!gameBootstrap)
                SceneManager.LoadScene(StartSceneName);
#endif
            Destroy(gameObject);
        }
    }
}