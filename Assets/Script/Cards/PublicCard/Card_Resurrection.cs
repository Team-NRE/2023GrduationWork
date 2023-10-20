using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ȱ
public class Card_Resurrection : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 3800;
        _cost = 3;

        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = default;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        if (_player.GetComponent<PlayerStats>().isResurrection == false ) 
        { 
            _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Resurrection", ground, Quaternion.identity);
            _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);
        }

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
