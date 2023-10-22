using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class InfectionStart : BaseEffect
{
    int playerId;
    
    int enemylayer = default;

    PhotonView _pv;

    PlayerStats pStats;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);
        _pv = GetComponent<PhotonView>();
        pStat = player.GetComponent<PlayerStats>();
        
        //ViewId 초기화
        playerId = userId;

        //Layer 초기화
        enemylayer = pStats.enemyArea;
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
        _pv.RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
    {
        //Trigger로 선별된 ViewId의 게임오브젝트 초기화
        GameObject other = Managers.game.RemoteTargetFinder(otherId);

        //오브젝트가 없다면 return
        if (other == null)
            return;

        //해당 오브젝트가 같은 팀이라면
        if (other.layer == enemylayer)
        {
            //타겟이 미니언, 타워일 시 
            if (!other.CompareTag("PLAYER"))
            {
                ObjStats oStats = other.GetComponent<ObjStats>();
                PlayerStats pStats = player.GetComponent<PlayerStats>();

                oStats.nowHealth -= pStats.basicAttackPower * 0.01f;
            }

            //타겟이 Player일 시
            if (other.CompareTag("PLAYER"))
            {
                PlayerStats enemyStats = other.GetComponent<PlayerStats>();
                PlayerStats pStats = player.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (playerId, pStats.basicAttackPower * 0.01f);
            }
        }
    }
}
