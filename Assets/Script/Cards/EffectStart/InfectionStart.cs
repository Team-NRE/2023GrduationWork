using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class InfectionStart : BaseEffect
{
    GameObject player = null;
    float damage = default;
    int enemylayer = default;
    protected PhotonView _pv;

    public void StartInfection(int _player, float _damage, int _enemylayer)
    {
        //player = GameObject.Find(_player);
        player = Managers.game.RemoteTargetFinder(_player);
        damage = _damage;
        enemylayer = _enemylayer;
        _pv = GetComponent<PhotonView>();
    }

    public void OnTriggerStay(Collider other)
    {
        int id = Managers.game.RemoteTargetIdFinder(other.gameObject);
        _pv.RPC("RpcTrigger", RpcTarget.All, id);   
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
	{
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        if (other.gameObject.layer == enemylayer)
        {
            Debug.Log(other.gameObject.name);

            //Ÿ���� �̴Ͼ�, Ÿ���� �� 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.05f);
            }

            //Ÿ���� �� Player�� ��
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = damage + (pStats.basicAttackPower * 0.05f);
                if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }
            }
        }
    }
}
