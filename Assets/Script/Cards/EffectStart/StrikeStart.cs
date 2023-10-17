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
    PlayerStats pStats;
    ObjStats oStats;

    float _originalpStat;
    float _originaloStat;

    bool IsEffect = false;

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId, targetId);
        _playerId = userId;
        _targetId = targetId;

        pStats = player.GetComponent<PlayerStats>();
        _enemyLayer = pStats.enemyArea;

        if (target.gameObject.CompareTag("OBJECT"))
        {
            _originaloStat = target.GetComponent<ObjStats>().speed;
        }
        else 
        {
            _originalpStat = target.GetComponent<PlayerStats>().speed;
        }

        this.gameObject.transform.parent = target.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 1.8f, 0);

        damage = 40.0f;
        effectTime = 2.0f;
    }

    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All, _playerId, _targetId, 5.0f);
    }

    [PunRPC]
    public IEnumerator RpcUpdate(int playerId, int targetId, float time)
	{
        GameObject playerObject = Managers.game.RemoteTargetFinder(playerId);
        GameObject targetObject = Managers.game.RemoteTargetFinder(targetId);


        if (targetObject.gameObject.CompareTag("OBJECT"))
        {
            ObjStats oStats = target.GetComponent<ObjStats>();
            
            _originaloStat = oStats.speed;

            oStats.nowHealth -= damage + (pStats.basicAttackPower * 0.7f);
            oStats.speed = 0;
        }
        else
        {
            PlayerStats enemyStats = target.GetComponent<PlayerStats>();

            _originalpStat = enemyStats.speed;

            enemyStats.receviedDamage = (_playerId, damage + (pStats.basicAttackPower * 0.7f));
            enemyStats.speed = 0;
        }

        yield return new WaitForSeconds(time);

        if (targetObject.gameObject.CompareTag("OBJECT"))
        {
            ObjStats oStats = target.GetComponent<ObjStats>();
            
            oStats.speed = 5;
        }

        else
        {
            PlayerStats pStats = target.GetComponent<PlayerStats>();
            
            pStats.speed = _originalpStat;
        }

        //PhotonNetwork.Destroy(this.gameObject);
    }
}
