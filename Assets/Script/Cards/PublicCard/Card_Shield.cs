using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class Card_Shield : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 300;
        _cost = 0;
        _defence = 50;
        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = 2.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.myCharacter;

        PlayerStats _pStat = _player.GetComponent<PlayerStats>();

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Shield");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Shield", ground, Quaternion.identity);
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 1.12f, 0);
        _effectObject.AddComponent<ShieldStart>().StartShield(player, _defence);

        _pStat.defensePower += _defence;

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
