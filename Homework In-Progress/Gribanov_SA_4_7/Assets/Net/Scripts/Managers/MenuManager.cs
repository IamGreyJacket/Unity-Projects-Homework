using Photon.Pun;
using UnityEngine;

namespace Net.Managers
{
    public class MenuManager : MonoBehaviourPunCallbacks
    {

        public void OnCreateRoom_UnityEditor()
        {
            PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2});
        }

        public void OnJoinRoom_UnityEditor()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void OnQuit_UnityEditor()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        Application.Quit();
#endif
        }

        // Start is called before the first frame update
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)] 
        void Start()
        {
            Debugger.OnStart();
#if UNITY_EDITOR
            PhotonNetwork.NickName = "1"; //Editor
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
            PhotonNetwork.NickName = "2"; //Client
#endif

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "0.0.1";
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debugger.Log("Ready for connecting");
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("NetGameScene");
        }
    }
}
