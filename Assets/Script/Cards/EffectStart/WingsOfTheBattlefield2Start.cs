using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsOfTheBattlefield2Start : BaseEffect
{
    protected PhotonView _pv;

    int teamLayer = default;


    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        ///�ʱ�ȭ
        base.CardEffectInit(userId);
        _pv = GetComponent<PhotonView>();
        teamLayer = player.GetComponent<PlayerStats>().playerArea;

        ///effect ��ġ
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 1.12f, 0);

        ///�ڱ� �ڽſ��� ���ο� effect �ν��Ͻ�ȭ 
        GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_WingsoftheBattlefield", player.transform);
        ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
    }

    ///���� effect�� collider Enter�� Object �Ǻ� 
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
            ///���� �������� ���ο� effect �ν��Ͻ� ȭ
            GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_WingsoftheBattlefield", other.transform);
            ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
        }
    }
}
