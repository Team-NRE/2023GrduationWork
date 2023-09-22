using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// ����
public class Card_Crystal : UI_Card
{
    protected PhotonView _pv;

    public override void Init()
    {
        _cardBuyCost = 400;
        _cost = 0;
        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = 2.0f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        _pv = GetComponent<PhotonView>();
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);
        PlayerStats _pStat = _player.GetComponent<PlayerStats>();

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Crystal");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Crystal", ground, Quaternion.Euler(-90, 0, 0));
        //_effectObject.transform.parent = _player.transform;
        //_pv.RPC("RemoteReparent", RpcTarget.All, playerId, _effectObject.GetComponent<PhotonView>().ViewID);
        //_effectObject.transform.localPosition = new Vector3(0, 0, 0);

        //_pStat.nowMana += 1*_pStat.manaRegen;
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
