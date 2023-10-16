using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

// ��ŷ ����ź
public class Card_HackingGrenade : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 1200;
        _cost = 1;

        _rangeType = Define.CardType.Point;
        _rangeScale = 3.15f;
        _rangeRange = 4.0f;

        _effectTime = 0.7f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_HackingGrenade", ground, Quaternion.identity);
        _effectObject.transform.position = new Vector3(ground.x, 0.2f, ground.z);

        _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId);
        
        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
