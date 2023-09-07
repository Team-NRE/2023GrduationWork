using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_WingsOfTheBattlefield : UI_Card
{  
    int _layer = default;
    float _speedTime = default;

    public override void Init()
    {
        _cardBuyCost = 2000;
        _cost = 2;
        _speed = 2.0f;
        _rangeScale = 3.6f;
        _rangeType = Define.CardType.None;

        _CastingTime = 0.3f;
        _effectTime = 1.1f;
        _speedTime = 5.0f;
    }

    public override GameObject cardEffect(Vector3 ground, string player, int layer = default)
    {
        //GameObject _player = GameObject.Find(player);
        GameObject _player = Managers.game.myCharacter;

        _layer = layer;
           
        //∂Ï∑Œ∏µ
        //_effectObject = Managers.Resource.Instantiate($"Particle/Effect_InvincibleShield");
        _effectObject = PhotonNetwork.Instantiate($"Prefabs/Particle/Effect_InvincibleShield", ground, Quaternion.Euler(-90, 0, 0));
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 1.12f, 0);


        //Speed ¿Ã∆Â∆Æ
        Collider[] cols = Physics.OverlapSphere(_player.transform.position, _rangeScale, 1 << _layer);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "PLAYER")
            {
                Debug.Log(col.gameObject.name);

                GameObject Wing = Managers.Resource.Instantiate($"Particle/Effect_WingsoftheBattlefield", col.transform);
                Wing.transform.localPosition = new Vector3(0, 1.0f, 0);
                Wing.AddComponent<WingsOfTheBattlefieldStart>().StartWings(col.gameObject.name, _speed, _speedTime);

                PlayerStats _pStat = col.gameObject.GetComponent<PlayerStats>();
                _pStat.speed += _speed;
            }

            else continue;
        }

        return _effectObject;
    }

    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
