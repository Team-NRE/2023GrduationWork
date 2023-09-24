using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// ��
public class Card_Armor : UI_Card
{
    PhotonView _pv;
    public override void Init()
    { 
        _cardBuyCost = 500;
        _cost = 0;
        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = 1.0f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        PlayerStats _pStat = _player.GetComponent<PlayerStats>();
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Armor", this.gameObject.transform.position, Quaternion.identity);
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
