using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Checkers
{
    public class ReplayRecorder : MonoBehaviour
    {
        private FileStream _replayRecordStream;
        private StreamWriter _replayRecordWriter;
        private StreamReader _replayRecordReader;
        private string _replayRecordName;
        private string _replayRecordPath;
        [SerializeField]
        private string _replayRecordFolderName = "Replays";
        public static ReplayRecorder Self { get; private set; }

        [SerializeField, Tooltip("The time between each move in replay")]
        private float _nextMoveDelay = 1f;
        [SerializeField]
        private bool _isRecording;
        [SerializeField]
        private bool _isReplay;
        public bool IsReplay => _isReplay;
        public bool IsRecording => _isRecording;
        public float NextMoveDelay => _nextMoveDelay;

        private void OnValidate()
        {
            if (_replayRecordFolderName == string.Empty || _replayRecordFolderName == "")
            {
                _replayRecordFolderName = "Replays";
                Debug.LogWarning("You have to set a name for a folder, it cannot be empty");
            }
        }

        private void Awake()
        {
            if (_isRecording) _isReplay = false;
            else if (_isReplay) _isRecording = false;
            else
            {
                Debug.LogWarning("You're neither recording a replay, nor playing it");
            }
            Self = this;
            try
            {
                _replayRecordName = $"Replay.txt";
                _replayRecordPath = $"{_replayRecordFolderName}\\{_replayRecordName}";
                _replayRecordStream = new FileStream(_replayRecordPath, FileMode.Open);
                _replayRecordStream.Close();
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\{_replayRecordFolderName}");
                _replayRecordStream = new FileStream(_replayRecordPath, FileMode.OpenOrCreate);
                _replayRecordStream.Close();
            }
            if (_isReplay) _replayRecordReader = new StreamReader(_replayRecordPath);
            else if(_isRecording) _replayRecordWriter = new StreamWriter(_replayRecordPath);
            
        }

        public string GetNextLine()
        {
            return _replayRecordReader.ReadLine();
        }

        public void WriteRecord(string text)
        {
            _replayRecordWriter.WriteLine($"{text}");
        }

        private void OnDestroy()
        {
            if(_isReplay) _replayRecordReader.Close();
            if(_isRecording) _replayRecordWriter.Dispose();
            _replayRecordStream.Dispose();
        }
    }
}
