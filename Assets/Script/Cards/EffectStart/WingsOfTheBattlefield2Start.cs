using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsOfTheBattlefield2Start : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //�ʱ�ȭ
        base.CardEffectInit(userId);
        effectPV = GetComponent<PhotonView>();
        
        //Layer �ʱ�ȭ
        teamLayer = pStat.playerArea;

        //effect ��ġ
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 1.12f, 0);

        //�ڱ� �ڽſ��� ���ο� effect �ν��Ͻ�ȭ 
        GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_WingsoftheBattlefield", player.transform);
        ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
    }

    //���� effect�� collider Enter�� Object �Ǻ� 
    public void OnTriggerEnter(Collider other)
    {
        //Human & Cyborg & Neutral �Ű�ü �� return
        if (other.gameObject.layer != (int)Define.Layer.Human && other.gameObject.layer != (int)Define.Layer.Cyborg
                && other.gameObject.layer != (int)Define.Layer.Neutral)
            return;

        //������ Collider�� ViewId ã�� 
        int otherId = Managers.game.RemoteColliderId(other);

        //�ش� ViewId�� default�� return
        if (otherId == default)
            return;

        //RPC ����
        effectPV.RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
    {
        //Trigger�� ������ ViewId�� ���ӿ�����Ʈ �ʱ�ȭ
        GameObject other = Managers.game.RemoteTargetFinder(otherId);

        //���� ������ �ƴϸ� return
        if (other == null) return;
        if (!other.CompareTag("PLAYER")) return;
        if (other.layer != teamLayer) return;
        if (other.layer == teamLayer && other.CompareTag("PLAYER"))
        {
            //���� �������� ���ο� effect �ν��Ͻ� ȭ
            GameObject ShieldEffect = Managers.Resource.Instantiate($"Particle/Effect_WingsoftheBattlefield", other.transform);
            ShieldEffect.transform.localPosition = new Vector3(0, 1.12f, 0);
        }
    }
}
