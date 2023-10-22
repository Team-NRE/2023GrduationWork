using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class BloodTransfusionStart : BaseEffect
{
    PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        //초기화
        base.CardEffectInit(userId, targetId);
        _pv = GetComponent<PhotonView>();

        //effect 위치
        transform.parent = target.transform;
        transform.localPosition = new Vector3(0, 0.8f, 0);

        //스텟 적용
        damage = 30.0f;
    }

    private void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
	{
        if (!target.CompareTag("PLAYER"))
        {
            ObjStats target_oStats = target.GetComponent<ObjStats>();

            target_oStats.nowHealth -= damage + (pStat.basicAttackPower * 0.7f);
            target_oStats.nowHealth += damage + (pStat.basicAttackPower * 0.7f);

            BaseCard._lockTarget = null;
            GetComponent<BloodTransfusionStart>().enabled = false;
        }

        if (target.CompareTag("PLAYER"))
        {
            PlayerStats target_pStats = target.GetComponent<PlayerStats>();

            target_pStats.receviedDamage = (playerId, (damage + (pStat.basicAttackPower * 0.7f)));
            pStat.nowHealth += damage + (pStat.basicAttackPower * 0.7f);

            BaseCard._lockTarget = null;
            GetComponent<BloodTransfusionStart>().enabled = false;
        }
    }
}
