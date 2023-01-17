using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Net.Managers
{
    public delegate void Notifier();
    public delegate void EndGameNotifier(PlayerController loser);
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public event Notifier OnGameOverEvent;
        private const byte ON_GAME_OVER_PHOTONEVENT = 0;

        private PlayerController _player1;
        private PlayerController _player2;

        public static GameManager Self;

        [SerializeField]
        private GameObject _winLoseMessageWindow;
        [SerializeField]
        private TextMeshProUGUI _winLoseText; 

        [SerializeField]
        private string _playerPrefabName;
        [SerializeField]
        private InputAction _exit;
        [SerializeField]
        private InputAction _quit;
        [SerializeField, Range(1f, 15f),
            Tooltip("В пределах каких координат будет создаваться игрок")]
        private float _randomInterval = 7f;

        public bool IsGameOver { get; private set; } = false;

        private void Awake()
        {
            Self = this;

            PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
        }

        private void NetworkingClient_EventReceived(EventData obj)
        {
            switch(obj.Code)
            {
                case 0:
                    //todo
                    IsGameOver = true;
                    var loser = (string)obj.CustomData;
                    OnGameOverEvent?.Invoke();
                    if (loser == "Player 1")
                    {
                        _winLoseText.text = "Player 1 loses, Player 2 Wins!";
                    }
                    else
                    {
                        _winLoseText.text = "Player 2 loses, Player 1 Wins!";
                    }
                    _winLoseMessageWindow.SetActive(true);
                    break;

            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _exit.Enable();
            _quit.Enable();
            _exit.performed += OnExit;
            _quit.performed += OnQuit;

            Self = this;

            Debugger.OnStart();

            var pos = new Vector3(Random.Range(-_randomInterval, _randomInterval), 0f, Random.Range(-_randomInterval, _randomInterval));
            var GO = PhotonNetwork.Instantiate(_playerPrefabName + PhotonNetwork.NickName, pos, new Quaternion());

            PhotonPeer.RegisterType(typeof(PlayerData), 100, Debugger.SerializePlayerData, Debugger.DeserializePlayerData);
        }

        public void AddPlayer(PlayerController player)
        {
            if (player.name.Contains("1")) _player1 = player;
            else _player2 = player;

            if(_player1 != null && _player2 != null)
            {
                _player1.SetTarget(_player2.transform);
                _player2.SetTarget(_player1.transform);
                _player1.OnPlayerLoseEvent += GameOver;
                _player2.OnPlayerLoseEvent += GameOver;
            }
        }

        public void GameOver(PlayerController player)
        {
            if (IsGameOver) return;
            IsGameOver = true;
            string loser;
            OnGameOverEvent?.Invoke();
            //to do Photon Event
            if(player == _player1)
            {
                _winLoseText.text = "Player 1 loses, Player 2 Wins!";
                loser = "Player 1";
            }
            else
            {
                _winLoseText.text = "Player 2 loses, Player 1 Wins!";
                loser = "Player 2";
            }
            _winLoseMessageWindow.SetActive(true);

            PhotonNetwork.RaiseEvent(ON_GAME_OVER_PHOTONEVENT, loser, Photon.Realtime.RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }

        private void OnQuit(InputAction.CallbackContext obj)
        {
            PhotonNetwork.LeaveRoom();
            //SceneManager.LoadScene("NetMenuScene");
        }

        public void OnQuit_Editor()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void OnExit(InputAction.CallbackContext obj)
        {
            PhotonNetwork.LeaveRoom();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        Application.Quit();
#endif

        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("NetMenuScene");
        }

        private void OnDestroy()
        {
            _exit.Dispose();
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
        }

    }
}
