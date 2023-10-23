using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using static UnityEngine.GraphicsBuffer;
using Photon.Pun;

public class BloodstainedCoinStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        //초기화
        base.CardEffectInit(userId, targetId);
        effectPV = GetComponent<PhotonView>();

        //스텟 적용
        powerValue = (50.0f, 1.0f);
        damageValue = powerValue.Item1 + (pStat.basicAttackPower * powerValue.Item2);
    }

    private void Update()
    {
        Vector3 thisPos = transform.position;
        Vector3 targetPos = target.transform.position;

        transform.position = Vector3.Slerp(transform.position, targetPos + Vector3.up, Time.deltaTime * 2.0f);

        if (Vector3.Distance(thisPos, targetPos) <= 1.5f)
        {
            //맞은 적 effect 적용
            GameObject _effectObject = PhotonNetwork.Instantiate("Prefabs/Particle/Effect_BloodstainedCoin", targetPos, Quaternion.identity);
            _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId, targetId);

            effectPV.RPC("ApplyDamage", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ApplyDamage()
    {
        //데미지 피해
        if (target.gameObject.CompareTag("PLAYER"))
        {
            target_pStat = target.GetComponent<PlayerStats>();
            target_pStat.receviedDamage = (playerId, damageValue);

            //적 사망 시 돈 추가 입금
            if (target_pStat.nowHealth <= 0)
            {
                pStat.gold += 300f;
            }

            Destroy(gameObject);
            this.enabled = false;

            return;
        }
        if (!target.gameObject.CompareTag("PLAYER"))
        {
            target_oStat = target.GetComponent<ObjStats>();
            target_oStat.nowHealth -= damageValue;

            Destroy(gameObject);
            this.enabled = false;

            return;
        }
    }
}
