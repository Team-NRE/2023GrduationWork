using Photon.Pun;
using Stat;
using UnityEngine;
using Define;

public class MissileBombStart : BaseEffect
{
    int attackID;
    float distance = 5.0f;

    [SerializeField]
    GameObject explosionParticle;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        attackID = userId;
        PlayerStats stats = player.GetComponent<PlayerStats>();
        _damage = stats.basicAttackPower * 3.5f;
    }

    public void TakeDamage()
    {
        explosionParticle.SetActive(true);

        Collider[] colls = Physics.OverlapSphere(
            transform.position, 
            distance,
            1 << (int)(player.layer == (int)Layer.Human ? Layer.Cyborg : Layer.Human) 
        );

        for (int i=0; i<colls.Length; i++) {
            Transform nowTarget = colls[i].transform;

            //타겟이 미니언, 타워일 시 
            if (nowTarget.tag != "PLAYER")
            {
                ObjStats _Stats = nowTarget.GetComponent<ObjStats>();
                _Stats.nowHealth -= _damage;
            }

            //타겟이 적 Player일 시
            if (nowTarget.tag == "PLAYER")
            {
                if (!GetComponent<PhotonView>().IsMine) return;
                nowTarget.GetComponent<PhotonView>().RPC(
                    "photonStatSet",
                    RpcTarget.All,
                    attackID,
                    "receviedDamage",
                    _damage
                );
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
