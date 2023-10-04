using UnityEngine;
using Photon.Pun;

public class SpecialCard_EnergyAmp : UI_Card
{
    int _layer = default;

    public override void Init()
    {
        _cost = 3;

        _rangeType = Define.CardType.None;
        _rangeScale = 3.0f;

        _CastingTime = 0.7f;
    }

    
    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/EffectSpecial_EnergyAmp", ground, Quaternion.identity);
        _effectObject.transform.position = ground;
        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);
        
        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
