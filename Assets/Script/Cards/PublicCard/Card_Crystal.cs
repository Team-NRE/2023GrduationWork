using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class Card_Crystal : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 400;
        _cost = 0;
        _rangeType = "None";

        _CastingTime = 0.3f;
        _effectTime = 1.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.myCharacter;

        PlayerStats _pStat = _player.GetComponent<PlayerStats>();

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Crystal");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Crystal", ground, Quaternion.identity);
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 0, 0);

        _pStat.nowMana += 1*_pStat.manaRegen;

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
