using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class Card_Speed : UI_Card
{
    public override void Init()
    {
        _cardBuyCost = 300;
        _cost = 0;
        _speed = 1.5f;
        _rangeType = "None";

        _CastingTime = 0.3f;
        _effectTime = 5.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.myCharacter;

        PlayerStats _pStat = _player.GetComponent<PlayerStats>();

        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_Speed");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_Speed", ground, Quaternion.identity);
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 0.2f, 0);
        _effectObject.AddComponent<SpeedStart>().StartSpeed(player, _speed);

        _pStat.speed += _speed;

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
