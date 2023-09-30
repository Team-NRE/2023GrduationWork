using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class JobCard_Charge : UI_Card
{
    public override void Init()
    {
        _cost = 1;

        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = 3.0f;
    }


    public override GameObject cardEffect(Vector3 ground, int playerId, int layer = default)
    {
        GameObject _player = Managers.game.myCharacter;
        PlayerStats _pStat = _player.GetComponent<PlayerStats>();

        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle2/EffectJob_Charge", ground, Quaternion.Euler(-90, 0, 0));

        _effectObject.GetComponent<PhotonView>().RPC(
            "CardEffectInit",
            RpcTarget.All,
            playerId
        );

        return _effectObject;
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
