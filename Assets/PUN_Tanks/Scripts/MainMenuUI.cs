using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;
using UnityEngine.UI;

namespace PUN_Tanks
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField if_RoomName;
        [SerializeField] TMP_Dropdown dd_playerColor;
        [SerializeField] GameObject lobbyPanel;
        [SerializeField] Button btn_CreateRoom;
        [SerializeField] Button btn_JoinRoom;
        [SerializeField] Button btn_StartGame;
        [SerializeField] Button btn_GetReady;

        private void Start()
        {
            NetworkManager.Instance.OnConnectedToServer += OnConnectedToServer;
            NetworkManager.Instance.OnRoomCreated += OnRoomCreated;
            NetworkManager.Instance.OnRoomJoinFailed += ResetButtons;

            lobbyPanel.SetActive(false);
            btn_StartGame.interactable = false;
            btn_GetReady.interactable = true;
        }

        private void OnRoomCreated()
        {
           
        }

        private void OnConnectedToServer()
        {
            lobbyPanel.SetActive(true);
        }

        public void OnNameUpdated(string pName)
        {
            PhotonNetwork.NickName = pName;
        }

        public void CreateRoom()
        {
            if (!string.IsNullOrEmpty(if_RoomName.text))
            {
                NetworkManager.Instance.CreateRoom(if_RoomName.text);
            }

            btn_CreateRoom.interactable = btn_JoinRoom.interactable = false;
        }

        public void JoinRoom()
        {
            if (!string.IsNullOrEmpty(if_RoomName.text))
            {
                NetworkManager.Instance.JoinRoom(if_RoomName.text);
            }

            btn_CreateRoom.interactable = btn_JoinRoom.interactable = false;
        }

        public void StartGame()
        {
            NetworkManager.Instance.LoadGameplayScene();
        }
        public void GetReady()
        {
            NetworkManager.Instance.AllReady();
            btn_GetReady.interactable = false;
            if (NetworkManager.Instance.isReady)
            {
                btn_StartGame.interactable = true;
            }
        }

        void ResetButtons()
        {
            btn_CreateRoom.interactable = btn_JoinRoom.interactable = true;
        }

        public void OnColorUpdated(int colorIdx)
        {
            Color pColor = Color.black;
            switch (colorIdx)
            {
                case 0:
                    pColor = Color.black;
                    break;
                case 1:
                    pColor = Color.blue;
                    break;
                case 2:
                    pColor = Color.red;
                    break;
            }

            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
            playerProps.Add("color_r", pColor.r);
            playerProps.Add("color_g", pColor.g);
            playerProps.Add("color_b", pColor.b);
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        }

        private void OnDestroy()
        {
            NetworkManager.Instance.OnConnectedToServer -= OnConnectedToServer;
            NetworkManager.Instance.OnRoomCreated -= OnRoomCreated;
            NetworkManager.Instance.OnRoomJoinFailed -= ResetButtons;
        }
    }

}