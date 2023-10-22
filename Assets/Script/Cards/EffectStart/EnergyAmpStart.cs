using Photon.Pun;
using Stat;
using UnityEngine;
using Define;

public class EnergyAmpStart : BaseEffect
{
    float distance;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);
        
        //스텟 적용
        distance = 5.0f;
        damage = pStat.basicAttackPower * 3.5f;
    }

    public void TakeDamage()
    {
        Collider[] colls = Physics.OverlapSphere(
            transform.position,
            distance,
            1 << (int)(player.layer == (int)Layer.Human ? Layer.Cyborg : Layer.Human)
        );

        for (int i = 0; i < colls.Length; i++)
        {
            Transform nowTarget = colls[i].transform;

            //타겟이 미니언, 타워일 시 
            if (nowTarget.tag != "PLAYER")
            {
                ObjStats target_oStats = nowTarget.GetComponent<ObjStats>();
                target_oStats.nowHealth -= damage;
            }

            //타겟이 적 Player일 시
            if (nowTarget.tag == "PLAYER")
            {
                PlayerStats target_pStats = nowTarget.GetComponent<PlayerStats>();
                target_pStats.receviedDamage = (playerId, damage);
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
