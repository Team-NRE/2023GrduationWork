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
        
        _enemyLayer = player.GetComponent<PlayerStats>().enemyArea;

        if (target.gameObject.CompareTag("OBJECT"))
        {
            ObjStats oStats = target.GetComponent<ObjStats>();
            PlayerStats pStats = target.GetComponent<PlayerStats>();
            _originaloStat = target.GetComponent<ObjStats>().speed;
            oStats.nowHealth -= 15.0f + pStats.basicAttackPower * 0.7f;
        }
        else 
        {
            PlayerStats myStats = Managers.game.RemoteTargetFinder(_playerId).GetComponent<PlayerStats>();
            PlayerStats pStats = target.GetComponent<PlayerStats>();
            _originalpStat = target.GetComponent<PlayerStats>().speed;
            pStats.receviedDamage = (_playerId, 15.0f + (myStats.basicAttackPower * 0.7f));

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
            oStats.speed = 0;
        }
        else
        {
            PlayerStats pStats = target.GetComponent<PlayerStats>();
            pStats.speed = 0;
        }

        yield return new WaitForSeconds(time);

        if (targetObject.gameObject.CompareTag("OBJECT"))
        {
            ObjStats oStats = target.GetComponent<ObjStats>();
            oStats.speed = _originaloStat;
        }

        else
        {
            PlayerStats pStats = target.GetComponent<PlayerStats>();
            pStats.speed = _originalpStat;
        }

        //PhotonNetwork.Destroy(this.gameObject);
    }
}
