using UnityEditor;

using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid.Managers.Assistants
{
    public class PauseController : MonoBehaviour
    {
        //[SerializeField]
        //private bool _isPlayer1;

        public void OnResume_Editor()
        {
            gameObject.SetActive(false);
            Managers.GameManager.Self.UnpauseGame();
        }

        public void OnRestart_Editor()
        {
            Debug.LogWarning("Restarting the level(scene)");
            Managers.LogManager.Self.Log("Restarting the level(scene)");
            DontDestroyOnLoad(LogManager.Self.gameObject);
            SceneManager.LoadScene("GamePlayScene");
            Time.timeScale = 1f;
        }

        public void OnExit_Editor()
        {
            Debug.LogWarning("Exiting the game");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}