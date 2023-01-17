using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Arkanoid.Managers
{
    public class LogManager : MonoBehaviour
    {
        private FileStream _logStream;
        private StreamWriter _logWriter;
        private string _logName;
        private string _logPath;
        [SerializeField]
        private string _logFolderName = "Logs";
        public static LogManager Self { get; private set; }

        private void OnValidate()
        {
            if(_logFolderName == string.Empty || _logFolderName == "")
            {
                _logFolderName = "Logs";
                Debug.LogWarning("You have to set a name for a folder, it cannot be empty");
            }
        }

        private void Awake()
        {
            Self = this;
            try
            {
                _logName = $"Log {System.DateTime.Now.ToString().Replace(".", "_").Replace(":", "_")}.txt";
                _logPath = $"{_logFolderName}\\{_logName}";
                _logStream = new FileStream(_logPath, FileMode.OpenOrCreate);
                _logStream.Close();
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\{_logFolderName}");
                _logStream = new FileStream(_logPath, FileMode.OpenOrCreate);
                _logStream.Close();
            }
            _logWriter = new StreamWriter(_logPath);
            Log($"Log file {_logName} is open");
            Log($"Current game version is {Application.version}");
        }

        private void Start()
        {
            Log("Game is launched");
        }

        public void Log(string text)
        {
            _logWriter.WriteLine($"[{System.DateTime.Now.TimeOfDay.ToString().Split('.')[0]}] {text}");
        }

        private void OnDestroy()
        {
            Log("Exiting the game");
            _logWriter.Dispose();
            _logStream.Dispose();
        }
    }
}
