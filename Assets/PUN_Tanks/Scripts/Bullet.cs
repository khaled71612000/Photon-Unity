using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace PUN_Tanks
{
    public class Bullet : MonoBehaviour
    {
        Rigidbody rb;
        [SerializeField] float Speed;
        [SerializeField] float Damage;
        public int OwnerActorNR { get; set; }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * Speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                NetworkPlayer player = other.GetComponent<NetworkPlayer>();
                if (player.photonView.IsMine)
                {
                    player.ApplyDamage(Damage); 
                }
            }
            Destroy(gameObject);
        }
    }
}
