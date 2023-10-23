using Photon.Pun;
using Stat;
using UnityEngine;
using Define;

public class MissileBombStart : BaseEffect
{
    float distance;

    [SerializeField]
    GameObject explosionParticle;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);
        
        //스텟 적용
        distance = 2.5f;
        powerValue = (0f, 3.5f);
        damageValue = powerValue.Item1 + (pStat.basicAttackPower * powerValue.Item2);
    }

    public void TakeDamage()
    {
        explosionParticle.SetActive(true);

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
                ObjStats _Stats = nowTarget.GetComponent<ObjStats>();
                _Stats.nowHealth -= damageValue;
            }

            //타겟이 Player일 시
            if (nowTarget.tag == "PLAYER")
            {
                PlayerStats _Stats = nowTarget.GetComponent<PlayerStats>();
                _Stats.receviedDamage = (playerId, damageValue);
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
