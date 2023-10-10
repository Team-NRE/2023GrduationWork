using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class BloodTransfusionStart : BaseEffect
{
    GameObject Obj = null;
    protected PhotonView _pv;
    protected int _playerId;
    protected int _targetId;

    float damage = default;

    private void Start()
    {
        _pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        base.CardEffectInit(userId, targetId);
        _playerId = userId;
        _targetId = targetId;
        
        Obj = target;

        transform.parent = target.transform;
        transform.localPosition = new Vector3(0, 0.8f, 0);

        damage = 30.0f;
    }

    private void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All, _playerId, _targetId);
    }

    [PunRPC]
    public void RpcUpdate(int playerId, int targetId)
	{
        Obj = Managers.game.RemoteTargetFinder(targetId);
        GameObject user = Managers.game.RemoteTargetFinder(playerId);

        //Ÿ���� �̴Ͼ�, Ÿ���� �� 
        if (!Obj.CompareTag("PLAYER"))
        {
            ObjStats oStats = Obj.GetComponent<ObjStats>();
            PlayerStats pStats = user.GetComponent<PlayerStats>();

            oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.7f);
            pStats.nowHealth += damage + (pStats.basicAttackPower * 0.7f);

            BaseCard._lockTarget = null;
            GetComponent<BloodTransfusionStart>().enabled = false;
        }

        //Ÿ���� �� Player�� ��
        if (Obj.CompareTag("PLAYER"))
        {
            PlayerStats enemyStats = Obj.GetComponent<PlayerStats>();
            PlayerStats pStats = user.GetComponent<PlayerStats>();

            enemyStats.receviedDamage = (playerId, (damage + (pStats.basicAttackPower * 0.7f)));
            pStats.nowHealth += damage + (pStats.basicAttackPower * 0.7f);

            BaseCard._lockTarget = null;
            GetComponent<BloodTransfusionStart>().enabled = false;
        }
    }
}
