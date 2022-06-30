using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

namespace PUN_Tanks
{
    public class NetworkPlayer : MonoBehaviourPun, IPunObservable
    {
        [Header("Movement")]
        Rigidbody rb;
        [SerializeField] float movSpeed;
        [SerializeField] float rotationSpeed;
        [SerializeField] Transform cannonPivot;

        [Header("HUD")]
        [SerializeField] TextMeshProUGUI txt_PlayerName;
        [SerializeField] Image healthBar;
        [SerializeField] float maxHealth;
        float currHealth;

        //[Header("Shooting")]
        [SerializeField] Bullet bulletPrefab;
        [SerializeField] Transform bulletSpawnPos;

        [SerializeField] Healer healPrefab;
        [SerializeField] float HealingSpawnCooldown;
        float healCooldown;
        bool isDead;
        

        void Start()
        {
            txt_PlayerName.text = photonView.Owner.NickName;
            if (photonView.IsMine)
            {
                rb = GetComponent<Rigidbody>();
                currHealth = maxHealth;
            }
            healCooldown = HealingSpawnCooldown;
            UpdateColor();
        }

        private void Update()
        {
            healCooldown += Time.deltaTime;
            if (photonView.IsMine && !isDead)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                Vector3 mov = new Vector3(rb.position.x + h * movSpeed * Time.deltaTime, rb.position.y, rb.position.z + v * movSpeed * Time.deltaTime);
                rb.MovePosition(mov);

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    cannonPivot.Rotate(0, rotationSpeed * Time.deltaTime, 0);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    cannonPivot.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    photonView.RPC(nameof(Shoot), RpcTarget.All, bulletSpawnPos.position, cannonPivot.rotation);
                }  
            }
            if (Input.GetKeyDown(KeyCode.H) && healCooldown > HealingSpawnCooldown)
            {
                photonView.RPC(nameof(Heal), RpcTarget.All, transform.position, transform.rotation);
            }
        }

        void UpdateColor()
        {
            float pColor_r = (float)photonView.Owner.CustomProperties["color_r"];
            float pColor_g = (float)photonView.Owner.CustomProperties["color_g"];
            float pColor_b = (float)photonView.Owner.CustomProperties["color_b"];
            Color pColor = new Color(pColor_r, pColor_g, pColor_b);
            GetComponent<MeshRenderer>().material.color = pColor;
        }

        [PunRPC]
        public void Shoot(Vector3 spawnPos, Quaternion rotation)
        {
            Bullet b = Instantiate(bulletPrefab, spawnPos, rotation);
            b.OwnerActorNR = photonView.OwnerActorNr;
        }
        [PunRPC]
        public void Heal(Vector3 spawnPos, Quaternion rotation)
        {
            Healer H = Instantiate(healPrefab, spawnPos, rotation);
            healCooldown = 0;
        }
        public void ApplyDamage(float damage)
        {
            if (currHealth > 0)
            {
                if (!photonView.IsMine)
                {
                    return;
                }
                currHealth -= damage;
            }
            else
                isDead = true;
        }
        public void ApplyHeal(float heal)
        {
            if (currHealth<100)
            {
                if (photonView.IsMine)
                {
                    currHealth += heal;
                    Debug.Log(currHealth);
                }
                else
                {
                    return;
                }
            }
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(currHealth);
            }
            else
            {
                currHealth = (float)stream.ReceiveNext();
            }
            UpdateHealthBar();
        }
        #region HUD
        void UpdateHealthBar()
        {
            healthBar.fillAmount = currHealth / maxHealth;
        }
        #endregion
    }
}
