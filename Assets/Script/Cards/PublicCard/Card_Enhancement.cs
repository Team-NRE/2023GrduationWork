using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class Card_Enhancement : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 300;
        _cost = 0;
        _damage = 15;
        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = 5.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.myCharacter;
        PlayerStats _pStat = _player.GetComponent<PlayerStats>();

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Enhancement");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Enhancement", ground, Quaternion.Euler(-90,0,0));
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 0.2f, 0);
        _effectObject.AddComponent<EnhancementStart>().StartEnhancement(player, _damage);

        _pStat.basicAttackPower += _damage;

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
