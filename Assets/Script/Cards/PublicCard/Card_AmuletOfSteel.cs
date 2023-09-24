using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ö����
public class Card_AmuletOfSteel : UI_Card
{
    float effectTime = 1.1f;
    
    public override void Init()
    {
        _cardBuyCost = 2000;
        _cost = 2;

        _rangeScale = 3.6f;
        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
    }

    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject _player = Managers.game.RemoteTargetFinder(playerId);

        Collider[] cols = Physics.OverlapSphere(_player.transform.position, _rangeScale, 1 << layer);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "PLAYER")
            {
                GameObject effect = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_AmuletofSteel", col.transform.position, Quaternion.Euler(-90, 0, 0));
                effect.GetComponent<PhotonView>().RPC(
                    "CardEffectInit",
                    RpcTarget.All,
                    col.GetComponent<PhotonView>().ViewID
                );
            }
        }

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
