using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class HackingGrenadeStart : BaseEffect
{
    int enemylayer = default;

    PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);
        _pv = GetComponent<PhotonView>();

        //Layer 초기화
        enemylayer = pStat.enemyArea;

        //스텟 적용
        damage = 75.0f;
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

        //해당 오브젝트가 다른 팀이라면
        if (other.layer == enemylayer || other.layer == (int)Define.Layer.Neutral)
        {
            //타겟이 미니언, 타워일 시 
            if (!other.CompareTag("PLAYER"))
            {
                ObjStats target_oStats = other.GetComponent<ObjStats>();

                target_oStats.nowHealth -= damage + (pStat.basicAttackPower * 1.0f);
            }

            //타겟이 Player일 시
            if (other.CompareTag("PLAYER"))
            {
                PlayerStats target_pStats = other.GetComponent<PlayerStats>();

                target_pStats.receviedDamage = (playerId, damage + (pStat.basicAttackPower * 1.0f));

                GameObject HackingEffect = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_HackingGrenade2", other.transform.position, Quaternion.identity);
                HackingEffect.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId, otherId);
            }
        }
    }
}
