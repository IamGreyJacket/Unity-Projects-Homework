using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Cars
{ 
    public class GameManager : MonoBehaviour
    {
        public static GameManager Self;

        public bool IsCountdown { get; private set; }
        public bool IsFinished { get; private set; } = false;

        private Animator _animator;

        [SerializeField, Min(0)]
        private int _countdown;
        public string DriverName = "ShitDriver";

        public void SetDriverName(string name) => DriverName = name;

        [SerializeField]
        private GameObject _leaderboardWindow;
        [SerializeField]
        private GameObject _nameInputFieldWindow;
        [SerializeField]
        private TextMeshProUGUI _leaderboardText;
        [SerializeField]
        private TextMeshProUGUI _timeText;
        [SerializeField]
        private TextMeshProUGUI _countdownText;

        [SerializeField, Min(1)]
        private int _maxLeaderboardRecords = 10;
        private List<string> _driverNames;
        private List<float> _driverTimes;
        private SortedDictionary<float, string> _driverRecords;
        private float _currentTime;
        private bool _isStopwatch;

        private void Awake()
        {
            Self = this;
            IsCountdown = true;
        }

        private void Start()
        {
            /*_driverNames = new List<string>(LeaderboardManager.Self.GetNames());
            if (_driverNames.Count > 10)
            {
                _driverNames.RemoveRange(10, _driverNames.Count - 10);
                _driverNames.Capacity = _maxLeaderboardPlayers;
            }
            _driverNames.Capacity = _maxLeaderboardPlayers;
            _driverTimes = new List<float>(LeaderboardManager.Self.GetTimes());
            if (_driverTimes.Count > 10) 
            {
                _driverTimes.RemoveRange(10, _driverTimes.Count - 10);
                _driverTimes.Capacity = 10;
            }*/

            _driverRecords = new SortedDictionary<float, string>(LeaderboardManager.Self.GetRecords());
            if (_driverRecords.Count > _maxLeaderboardRecords - 1)
            {
                var keys = new List<float>(_driverRecords.Keys);
                for (int i = keys.Count - 1; i > _maxLeaderboardRecords - 2; i--)
                {
                    _driverRecords.Remove(keys[i]);
                }
            }
            _animator = _countdownText.gameObject.GetComponent<Animator>();
            StartCoroutine(Countdown());

        }

        private IEnumerator Countdown()
        {
            _countdownText.gameObject.SetActive(true);
            while (_countdown > 0)
            {
                _countdownText.text = _countdown.ToString();
                _animator.Play("CountdownAnimation");
                yield return new WaitForSeconds(.9f);
                _countdown--;
            }
            IsCountdown = false;
            _countdownText.text = "START!";
            _animator.Play("StartFinishAnimation");
            yield return new WaitForSeconds(2f);
            _countdownText.gameObject.SetActive(false);
            var temp = _countdownText.color;
            temp.a = 1;
            _countdownText.color = temp;
        }

        public void StartStopwatch()
        {
            _currentTime = 0;
            _isStopwatch = true;
            Debug.Log("Stopwatch is running");
        }

        public void StopStopwatch()
        {
            _isStopwatch = false;
            Debug.Log("Stopwatch is stopped at " + _timeText.text);
        }

        private void UpdateStopwatch()
        {
            _currentTime += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(_currentTime);
            int minutes = (int)time.TotalMinutes;
            if (minutes > 99) minutes = 99;
            _timeText.text = $"{minutes}:{time.ToString(@"ss\:f")}";

            //_timeText.text = ;
        }

        public IEnumerator Finish()
        {
            IsFinished = true;
            _countdownText.text = "FINISH!";
            _countdownText.gameObject.SetActive(true);
            _animator.Play("StartFinishAnimation");
            yield return new WaitForSeconds(1.5f);

            /*_driverTimes.Add(_currentTime);
            _driverNames.Add(DriverName);
            //_driverTimes.Sort();

            _leaderboardText.text = "";

            for (int i = 0; i < _driverTimes.Count; i++)
            {
                TimeSpan time = TimeSpan.FromSeconds(_driverTimes[i]);
                int minutes = (int)time.TotalMinutes;
                if (minutes > 99) minutes = 99;
                var res = $"{minutes}:{time.ToString(@"ss\:f")}";
                _leaderboardText.text += $"{res} - {_driverNames[i]}\n";
            }
            _leaderboardWindow.SetActive(true);

            for (int i = 0; i < _driverTimes.Count; i++)
            {
                LeaderboardManager.Self.Write($"{_driverTimes[i]} {_driverNames[i]}");
            }
            LeaderboardManager.Self.WriteTime(_driverNames.ToArray(), _driverTimes.ToArray());
            */
            _nameInputFieldWindow.SetActive(true);
            while (_nameInputFieldWindow.activeSelf)
            {
                yield return new WaitForSeconds(.3f);
            }
            if (DriverName == null || DriverName == string.Empty || DriverName == "")
            {
                DriverName = "DefaultPlayer";
            }

            _driverRecords[_currentTime] = DriverName;

            _leaderboardText.text = "";

            foreach (var timeKey in _driverRecords.Keys)
            {
                TimeSpan time = TimeSpan.FromSeconds(timeKey);
                int minutes = (int)time.TotalMinutes;
                if (minutes > 99) minutes = 99;
                var res = $"{minutes}:{time.ToString(@"ss\:f")}";
                _leaderboardText.text += $"{res} - {_driverRecords[timeKey]}\n";
            }
            _leaderboardWindow.SetActive(true);

            var names = new List<string>(_driverRecords.Values).ToArray();
            var times = new List<float>(_driverRecords.Keys).ToArray();
            LeaderboardManager.Self.WriteTime(names, times);
            //Debug.Log($"Showing the leaderboard");
        }

        private void Update()
        {
            if (_isStopwatch)
            {
                UpdateStopwatch();
            }
        }

    }
}