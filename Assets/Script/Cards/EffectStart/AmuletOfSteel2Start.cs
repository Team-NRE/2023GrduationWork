using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletOfSteel2Start : BaseEffect
{
    protected PhotonView _pv;

    int teamLayer = default;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        ///초기화
        base.CardEffectInit(userId);
        _pv = GetComponent<PhotonView>();
        
        //Layer 초기화
        teamLayer = pStat.playerArea;

        ///effect 위치
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 1.12f, 0);

        ///자기 자신에게 새로운 effect 인스턴스화 
        GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_AmuletofSteel", player.transform);
        ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
    }

    ///현재 effect의 collider Enter된 Object 판별 
    public void OnTriggerEnter(Collider other)
    {
        int otherId = Managers.game.RemoteColliderId(other);
        if (otherId == default)
            return;
        _pv.RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
    {
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        if (!other.CompareTag("PLAYER")) return;
        if (other.layer != teamLayer) return;
        if (other.layer == teamLayer && other.CompareTag("PLAYER"))
        {
            ///같은 팀원에게 새로운 effect 인스턴스 화
            GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_AmuletofSteel", other.transform);
            ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
        }
    }
}