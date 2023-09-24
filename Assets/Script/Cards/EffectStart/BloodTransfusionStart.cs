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

    float damage = default;

    private void Start()
    {
        _pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        //player = GameObject.Find(_player);
        _playerId = userId;
        player = Managers.game.RemoteTargetFinder(userId);
        Obj = transform.parent.gameObject;
        //_pv = GetComponent<PhotonView>();

        damage = 30.0f;
    }

    private void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All, _playerId);
    }

    [PunRPC]
    public void RpcUpdate(int playerId)
	{
        GameObject user = Managers.game.RemoteTargetFinder(playerId);

        //Ÿ���� �̴Ͼ�, Ÿ���� �� 
        if (Obj.tag != "PLAYER")
        {
            ObjStats oStats = Obj.GetComponent<ObjStats>();
            PlayerStats pStats = user.GetComponent<PlayerStats>();

            oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.7f);
            pStats.nowHealth += damage + (pStats.basicAttackPower * 0.7f);

            BaseCard._lockTarget = null;
            GetComponent<BloodTransfusionStart>().enabled = false;
        }

        //Ÿ���� �� Player�� ��
        if (Obj.tag == "PLAYER")
        {
            PlayerStats enemyStats = Obj.GetComponent<PlayerStats>();
            PlayerStats pStats = user.GetComponent<PlayerStats>();

            enemyStats.receviedDamage = (damage + (pStats.basicAttackPower * 0.7f));
            pStats.nowHealth += damage + (pStats.basicAttackPower * 0.7f);
            if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }

            BaseCard._lockTarget = null;
            GetComponent<BloodTransfusionStart>().enabled = false;
        }
    }
}
