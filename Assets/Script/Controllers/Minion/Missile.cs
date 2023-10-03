/// ksPark
///
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Stat;

public class Missile : MonoBehaviour
{
    PhotonView attackPV;
    PhotonView targetPV;

    float damage;
    float distance = 5.0f;

    [SerializeField]
    GameObject explosionParticle;

    [PunRPC]
    public void SummonMissile(int attackID, int targetID, float dis = 5.0f)
    {
        attackPV = PhotonView.Find(attackID);
        targetPV = PhotonView.Find(targetID);
        transform.position = targetPV.transform.position;
        damage = (attackPV.gameObject.layer == LayerMask.NameToLayer("Neutral") ? 
                    attackPV.GetComponent<ObjStats>().basicAttackPower : attackPV.GetComponent<PlayerStats>().basicAttackPower) * 3.5f;
        distance = dis;

        this.gameObject.SetActive(true);
    }

    public void TakeDamage()
    {
        explosionParticle.SetActive(true);

        Collider[] colls = Physics.OverlapSphere(
            transform.position, 
            distance,
            (1 << LayerMask.NameToLayer("Cyborg")) | (1 << LayerMask.NameToLayer("Human")) 
        );

        for (int i=0; i<colls.Length; i++) {
            Transform nowTarget = colls[i].transform;

            //타겟이 미니언, 타워일 시 
            if (nowTarget.tag != "PLAYER")
            {
                ObjStats _Stats = nowTarget.GetComponent<ObjStats>();
                _Stats.nowHealth -= damage;
            }

            //타겟이 적 Player일 시
            if (nowTarget.tag == "PLAYER")
            {
                if (!GetComponent<PhotonView>().IsMine) return;
                nowTarget.GetComponent<PhotonView>().RPC(
                    "photonStatSet",
                    RpcTarget.All,
                    attackPV.ViewID,
                    "receviedDamage",
                    damage
                );
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
