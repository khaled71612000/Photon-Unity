using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace PUN_Tanks
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        static NetworkManager instance;
        public event Action OnConnectedToServer;
        public event Action OnRoomCreated;
        public event Action OnRoomJoinFailed;
        [SerializeField] TextMeshProUGUI txt_Log;

        public bool isReady;

        public static NetworkManager Instance => instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            ConnectToNetwork();
        }

        void ConnectToNetwork()
        {
            PhotonNetwork.GameVersion = "v0.1";
            PhotonNetwork.ConnectUsingSettings();
            Log("Connecting...");
        }

        #region Callbacks

        public override void OnConnectedToMaster()
        {
            Log("Connected to Master");

            OnConnectedToServer?.Invoke();
        }

        public override void OnCreatedRoom()
        {
            Log($"Room {PhotonNetwork.CurrentRoom.Name} is created");
            OnRoomCreated?.Invoke();
        }

        public override void OnJoinedRoom()
        {
            Log($"Room {PhotonNetwork.CurrentRoom.Name} is joined");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Log($"Creating Room Failed {message}");
            OnRoomJoinFailed?.Invoke();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Log($"Join Room Failed {message}");
            OnRoomJoinFailed?.Invoke();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Log($"{newPlayer.NickName} joined the room");
        }

        #endregion

        public void CreateRoom(string roomName)
        {
            PhotonNetwork.CreateRoom(roomName, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void LoadGameplayScene()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("Only MasterClient should load gameplay scene");
            }

            PhotonNetwork.LoadLevel("Gameplay");
        }

        public void AllReady()
        {
            if(PhotonNetwork.IsMasterClient)
            {
                isReady = true;
            }
        }

        public void Log(string msg)
        {
            txt_Log.text = $"{msg}\n{txt_Log.text}";
        }

        public void ClearLog()
        {
            txt_Log.text = string.Empty;
        }
    } 
}
