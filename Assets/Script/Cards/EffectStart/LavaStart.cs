using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class LavaStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);
        effectPV = GetComponent<PhotonView>();

        //Layer 초기화
        enemyLayer = pStat.enemyArea;

        //스텟 적용
        powerValue = (0f, 0.02f);
        damageValue = powerValue.Item1 + (pStat.basicAttackPower * powerValue.Item2);
    }

    public void OnTriggerStay(Collider other)
    {
        //Human & Cyborg & Neutral 매개체 외 return
        if (other.gameObject.layer != (int)Define.Layer.Human && other.gameObject.layer != (int)Define.Layer.Cyborg
                && other.gameObject.layer != (int)Define.Layer.Neutral)
            return;

        //접근한 Collider의 ViewId 찾기 
        int otherId = Managers.game.RemoteColliderId(other);

        //해당 ViewId가 default면 return
        if (otherId == default)
            return;

        //RPC 적용
        effectPV.RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
    {
        //Trigger로 선별된 ViewId의 게임오브젝트 초기화
        GameObject other = Managers.game.RemoteTargetFinder(otherId);

        //오브젝트가 없다면 return
        if (other == null)
            return;

        //해당 오브젝트가 다른 팀이라면
        if (other.layer == enemyLayer || other.layer == (int)Define.Layer.Neutral)
        {
            //타겟이 미니언, 타워일 시 
            if (!other.CompareTag("PLAYER"))
            {
                ObjStats target_oStats = other.GetComponent<ObjStats>();

                target_oStats.nowHealth -= damageValue;
            }

            //타겟이 Player일 시
            if (other.CompareTag("PLAYER"))
            {
                PlayerStats target_pStats = other.GetComponent<PlayerStats>();

                target_pStats.receviedDamage = (playerId, damageValue);
            }
        }
    }
}
