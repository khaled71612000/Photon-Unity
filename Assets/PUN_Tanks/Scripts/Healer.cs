using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PUN_Tanks
{
    public class Healer : MonoBehaviour
    {
        [SerializeField] float healPeriod;
        [SerializeField] float healAmount;
        float healingTime;
        bool healing;
        NetworkPlayer cPlayer;
        private void Awake()
        {
            
        }
        void Start()
        {
            Destroy(this.gameObject, 3.0f);
        }
        private void Update()
        {
            healingTime += Time.deltaTime;
            if(healing && healingTime >= healPeriod)
            {
                if (cPlayer.photonView.IsMine)
                {
                    cPlayer.ApplyHeal(healAmount);
                }
                healingTime = 0;
            }    
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                cPlayer = other.GetComponent<NetworkPlayer>();
                healing = true;
            }   
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                healing = false;
            }
        }
    } 
}
