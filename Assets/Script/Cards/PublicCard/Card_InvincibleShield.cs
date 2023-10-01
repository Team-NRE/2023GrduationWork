using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Data;
using Photon.Pun;

// 무적 방패
public class Card_InvincibleShield : UI_Card
{

    public override void Init()
    {
        _cardBuyCost = 3333;
        _cost = 3;

        _rangeType = Define.CardType.None;
        _rangeScale = 3.6f;

        _CastingTime = 0.3f;
        _effectTime = 1.1f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        //띠로링
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_InvincibleShield", ground, Quaternion.identity);
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);
        return _effectObject;
    }

    private void Update()
    {

    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
