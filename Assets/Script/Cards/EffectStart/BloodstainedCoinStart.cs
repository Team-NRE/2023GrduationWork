using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using static UnityEngine.GraphicsBuffer;
using Photon.Pun;

public class BloodstainedCoinStart : BaseEffect
{
    GameObject _effectObject;
    protected PhotonView _pv;
    protected int _playerId;
    protected int _targetId;

    //Transform target = null;

    float damage = default;

    PlayerStats enemyStats;

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId, targetId);

        damage = 10.0f;
        _playerId = userId;
        _targetId = targetId;
    }

    private void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All, _playerId, _targetId);
    }

    [PunRPC]
    public void RpcUpdate(int playerId, int targetId)
    {
        // 동전 추적시키기
        GameObject player = Managers.game.RemoteTargetFinder(playerId);
        GameObject target = Managers.game.RemoteTargetFinder(targetId);

        Vector3 thisPos = this.gameObject.transform.position;
        Vector3 targetPos = target.transform.position;

        transform.position = Vector3.Slerp(transform.position, targetPos + Vector3.up, Time.deltaTime * 2.0f);

        if (Vector3.Distance(thisPos, targetPos) <= 1.5f)
        {
            _effectObject = PhotonNetwork.Instantiate("Prefabs/Particle/Effect_BloodstainedCoin", targetPos, Quaternion.identity);
            _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, _playerId, _targetId);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
