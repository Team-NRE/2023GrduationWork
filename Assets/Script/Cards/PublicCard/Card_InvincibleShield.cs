using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class Card_InvincibleShield : UI_Card
{
    string _playerName;
    GameObject _player;

    LayerMask _layer = default;

    private bool isShieldOn = false;

    public override void Init()
    {
        _cardBuyCost = 3333;
        _cost = 3;
        _defence = 99999;

        _rangeType = "None";
        _rangeScale = 6.0f;

        _CastingTime = 0.3f;
        _effectTime = 1.1f;
    }


    public override void UpdateInit()
    {
        if (_player != null && _layer != default && isShieldOn == true)
        {
            ShieldOn();
            isShieldOn = false; // 호출 상태를 true로 변경
        }
    }


    public override GameObject cardEffect(Vector3 ground, string player, LayerMask layer = default)
    {
        _playerName = player;
        _player = GameObject.Find(player);

        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_InvincibleShield");
        _effectObject.transform.parent = _player.transform;
        _effectObject.transform.localPosition = new Vector3(0, 1.12f, 0);

        _layer = layer;

        isShieldOn = true;

        return _effectObject;
    }


    public void ShieldOn()
    {
        Collider[] cols = Physics.OverlapSphere(_player.transform.position, 2 * _rangeScale, 1 << _layer);
        foreach (Collider col in cols)
        {
            //col.transform -> Police, 미니언
            GameObject shield = Managers.Resource.Instantiate($"Particle/Effect_InvincibleShield_1", col.transform);
            shield.AddComponent<InvincibleShieldStart>().Invincibility(_playerName, _defence, 1.5f, 3.0f);

            _player.gameObject.GetComponent<PlayerStats>().defensePower += _defence;
        }
    }


    public override void DestroyCard(float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
