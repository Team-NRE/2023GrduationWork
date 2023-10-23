using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKitStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);
        effectPV = GetComponent<PhotonView>();

        //effect 위치
        transform.parent = player.transform;
        
        //Layer 초기화
        teamLayer = pStat.playerArea;
        
        //스텟 적용
        healthRegenValue = 1f;
    }

    public void Update()
    {
        effectPV.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
    {
        pStat.nowHealth += healthRegenValue;
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

        //해당 오브젝트가 같은 팀이라면
        if (other.layer == teamLayer)
        {
            //타겟이 미니언, 타워일 시 
            if (!other.CompareTag("PLAYER"))
            {
                ObjStats target_oStats = other.GetComponent<ObjStats>();
                target_oStats.nowHealth += healthRegenValue;
            }

            //타겟이 Player일 시
            if (other.CompareTag("PLAYER"))
            {
                PlayerStats target_pStats = other.GetComponent<PlayerStats>();
                target_pStats.nowHealth += healthRegenValue;
            }
        }
    }
}
