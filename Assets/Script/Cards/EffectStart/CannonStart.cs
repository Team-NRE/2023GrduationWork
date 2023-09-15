using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class CannonStart : BaseEffect
{
    GameObject player = null;
    float damage = default;
    int enemylayer = default;
    protected PhotonView _pv;


    public void StartCannon(int playerId, float _damage, int _enemylayer)
    {
        //player = GameObject.Find(_player);
        player = Managers.game.RemoteTargetFinder(playerId);

        damage = _damage;
        enemylayer = _enemylayer;
    }

    // 여긴 백퍼 버그가 발생할 예정
    public void OnTriggerEnter(Collider other)
    {
        int colliderId = other.GetComponent<PhotonView>().ViewID;
        _pv.RPC("RpcTrigger", RpcTarget.All, colliderId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
	{
        GameObject other = Managers.game.RemoteTargetFinder(otherId);
        if (other.gameObject.layer == enemylayer)
        {
            Debug.Log(other.gameObject.name);

            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                ObjStats oStats = other.gameObject.GetComponent<ObjStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.5f);
            }

            //타겟이 적 Player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                PlayerStats enemyStats = other.gameObject.GetComponent<PlayerStats>();
                PlayerStats pStats = player.gameObject.GetComponent<PlayerStats>();

                enemyStats.receviedDamage = (damage + (pStats.basicAttackPower * 0.5f));
                if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }
            }
        }
    }
}
