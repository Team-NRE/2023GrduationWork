using UnityEngine;
using Photon.Pun;

public class SpecialCard_MissileBomb : UI_Card
{
    int _layer = default;
    int _enemylayer = default;

    public override void Init()
    {
        _cost = 3;

        _rangeType = Define.CardType.Point;
        _rangeScale = 3.0f;
        _rangeRange = 5.0f;

        _CastingTime = 0.7f;
    }

    
    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/EffectSpecial_MissileBomb", ground, Quaternion.identity);
        _effectObject.transform.position = ground;

        _layer = layer;

        if (_layer == 6) { _enemylayer = 7; }
        if (_layer == 7) { _enemylayer = 6; }

        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);
        
        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
