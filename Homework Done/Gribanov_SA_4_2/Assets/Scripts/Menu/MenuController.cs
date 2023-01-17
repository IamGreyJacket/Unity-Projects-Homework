using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

using UnityEditor;

namespace Arkanoid
{
    public class MenuController : MonoBehaviour
    {
        private string _gamePlaySceneName = "GamePlayScene";
        private GameObject _logManager;

        public void Awake()
        {
            _logManager = Arkanoid.Managers.LogManager.Self.gameObject;
        }

        public void OnNewGame_Editor()
        {
            Debug.Log("Start loading the level");
            Managers.LogManager.Self.Log("Start loading the level");
            StartCoroutine(LoadMyAsyncScene());
        }

        public void OnExit_Editor()
        {
            Debug.Log("Exiting the game");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private IEnumerator LoadMyAsyncScene()
        {
            // Set the current Scene to be able to unload it later
            Scene currentScene = SceneManager.GetActiveScene();

            // The Application loads the Scene in the background at the same time as the current Scene.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_gamePlaySceneName, LoadSceneMode.Additive);

            // Wait until the last operation fully loads to return anything
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
            SceneManager.MoveGameObjectToScene(_logManager, SceneManager.GetSceneByName(_gamePlaySceneName));
            // Unload the previous Scene
            SceneManager.UnloadSceneAsync(currentScene);
            Debug.Log("The level is loaded");
            Managers.LogManager.Self.Log("The level is loaded");
        }
    }
}
