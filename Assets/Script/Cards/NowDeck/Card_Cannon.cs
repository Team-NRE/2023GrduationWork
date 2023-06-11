using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class Card_Cannon : UI_Card
{
    Transform _Ground = null;
    Transform _Player = null;
    LayerMask _layer = default;
    LayerMask _enemylayer = default;
    bool isCannon = false;

    public override void Init()
    {
        _cost = 2;
        _damage = 50;
        _rangeType = "Point";
        _rangeScale = 1.5f;
        _rangeRange = 4.0f;

        _CastingTime = 0.7f;
        _effectTime = 1.0f;
    }

    public override void UpdateInit()
    {
        if (_Ground != null && _Player != default && _enemylayer != default && !isCannon)
        {
            CannonOn(_Ground, _Player, _enemylayer);
            _Ground = null;
            _Player = default;
            _enemylayer = default;
            isCannon = true; // 호출 상태를 true로 변경
        }
    }


    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_Cannon", Ground);

        _Ground = Ground;
        _Player = Player;
        _layer = layer;

        if (_layer == 1 << 6) { _enemylayer = 1 << 7; }
        if (_layer == 1 << 7) { _enemylayer = 1 << 6; }

        return _effectObject;
    }

    public void CannonOn(Transform Ground, Transform Player, LayerMask Enemylayer)
    {
        Collider[] cols = Physics.OverlapSphere(Ground.position, 2 * _rangeScale, Enemylayer);
        foreach (Collider col in cols)
        {
            //타겟이 미니언, 타워일 시 
            if (col.gameObject.tag != "PLAYER")
            {
                ObjStats _Stats = col.gameObject.GetComponent<ObjStats>();
                PlayerStats _pStats = Player.gameObject.GetComponent<PlayerStats>();

                _Stats.nowHealth -= (_damage + (_pStats.basicAttackPower * 0.5f));
            }

            //타겟이 적 Player일 시
            if (col.gameObject.tag == "PLAYER")
            {
                PlayerStats _EnemyStats = col.gameObject.GetComponent<PlayerStats>();
                PlayerStats _pStats = Player.gameObject.GetComponent<PlayerStats>();

                _EnemyStats.nowHealth -= (_damage + (_pStats.basicAttackPower * 0.5f));
            }
        }
    }


    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}