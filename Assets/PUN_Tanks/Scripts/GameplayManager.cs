using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PUN_Tanks
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] List<Transform> spawnPositions;

        void Start()
        {
            SpawnPlayer();
        }

        void SpawnPlayer()
        {
            int spawnIdx = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % PhotonNetwork.CurrentRoom.PlayerCount;
            Vector3 spawnPos = spawnPositions[spawnIdx].position;
            PhotonNetwork.Instantiate("MyPlayer", spawnPos, Quaternion.identity);
        }
    }
}
