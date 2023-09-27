using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class StrikeStart : BaseEffect
{
    protected PhotonView _pv;

    float damage = default;
    float effectTime = default;
    float StartEffect = 0.01f;
    float saveSpeed = default;

    int _targetId;
    int _playerId;
    int _enemyLayer;

    PlayerStats enemyStats;
    ObjStats oStats;

    bool IsEffect = false;

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId, targetId);
        _targetId = targetId;
        _enemyLayer = player.GetComponent<PlayerStats>().enemyArea;

        this.gameObject.transform.parent = target.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 1.8f, 0);

        damage = 40.0f;
        effectTime = 2.0f;
    }

    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All, _playerId, _targetId);
    }

    [PunRPC]
    public void RpcUpdate(int playerId, int targetId)
	{
        GameObject targetObj = Managers.game.RemoteTargetFinder(targetId);
        if (player == null) { PhotonNetwork.Destroy(gameObject); }
        if (player != null)
        {
            switch (IsEffect)
            {
                case false:
                    //Ÿ���� �̴Ͼ�, Ÿ���� �� 
                    if (targetObj.tag != "PLAYER")
                    {
                        oStats = targetObj.GetComponent<ObjStats>();
                        PlayerStats pStats = player.GetComponent<PlayerStats>();

                        oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.5f);

                        saveSpeed = oStats.speed;

                        oStats.speed = 0;

                        IsEffect = true;

                        break;
                    }

                    //Ÿ���� �� Player�� ��
                    if (targetObj.tag == "PLAYER")
                    {
                        enemyStats = targetObj.GetComponent<PlayerStats>();
                        PlayerStats pStats = player.GetComponent<PlayerStats>();

                        enemyStats.receviedDamage = (_targetId, (damage + (pStats.basicAttackPower * 0.5f)));
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
                        //Ÿ���� �̴Ͼ�, Ÿ���� �� 
                        if (targetObj.tag != "PLAYER")
                        {
                            oStats.speed = saveSpeed;

                            PhotonNetwork.Destroy(gameObject);
                        }

                        //Ÿ���� �̴Ͼ�, Ÿ���� �� 
                        if (targetObj.tag == "PLAYER")
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
