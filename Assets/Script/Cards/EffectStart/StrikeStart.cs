using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class StrikeStart : BaseEffect
{
    GameObject player = null;
    GameObject Obj = null;
    protected PhotonView _pv;

    float damage = default;
    float effectTime = default;
    float StartEffect = 0.01f;
    float saveSpeed = default;

    PlayerStats enemyStats;
    ObjStats oStats;

    bool IsEffect = false;

    public override void CardEffectInit(int userId, int targetId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId, targetId);
        Obj = target;

        damage = 40.0f;
        effectTime = 2.0f;
    }

    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
	{
        if (BaseCard._lockTarget == null) { Destroy(gameObject); }
        if (BaseCard._lockTarget != null)
        {
            switch (IsEffect)
            {
                case false:
                    //타겟이 미니언, 타워일 시 
                    if (Obj.tag != "PLAYER")
                    {
                        oStats = Obj.GetComponent<ObjStats>();
                        PlayerStats pStats = player.GetComponent<PlayerStats>();

                        oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.5f);

                        saveSpeed = oStats.speed;

                        oStats.speed = 0;

                        IsEffect = true;

                        break;
                    }

                    //타겟이 적 Player일 시
                    if (Obj.tag == "PLAYER")
                    {
                        enemyStats = Obj.GetComponent<PlayerStats>();
                        PlayerStats pStats = player.GetComponent<PlayerStats>();

                        enemyStats.receviedDamage = (damage + (pStats.basicAttackPower * 0.5f));
                        if (enemyStats.nowHealth <= 0) { pStats.kill += 1; }

                        saveSpeed = enemyStats.speed;
                        enemyStats.speed = 0;

                        IsEffect = true;

                        break;
                    }

                    break;

                case true:
                    StartEffect += Time.deltaTime;

                    if (StartEffect > effectTime - 0.01f)
                    {
                        //타겟이 미니언, 타워일 시 
                        if (Obj.tag != "PLAYER")
                        {
                            oStats.speed = saveSpeed;

                            PhotonNetwork.Destroy(gameObject);
                        }

                        //타겟이 미니언, 타워일 시 
                        if (Obj.tag == "PLAYER")
                        {
                            enemyStats.speed = saveSpeed;

                            PhotonNetwork.Destroy(gameObject);
                        }
                    }


                    break;
            }

        }
    }
}
