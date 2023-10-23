using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleShield2Start : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);
        effectPV = GetComponent<PhotonView>();

        //Layer 초기화
        teamLayer = pStat.playerArea;

        //effect 위치
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 1.12f, 0);

        //effect 인스턴스
        GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_InvincibleShield", player.transform);
        ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
    }

    public void OnTriggerEnter(Collider other)
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

        if (other.tag != "PLAYER") return;
        if (other.layer != teamLayer) return;
        if (other.layer == teamLayer && other.tag == "PLAYER")
        {
            //effect 인스턴스
            GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_InvincibleShield", other.transform);
            ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
        }
    }
}
