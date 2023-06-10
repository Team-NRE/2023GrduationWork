using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class Card_HackingGrenade : UI_Card
{
    Transform _Ground = null;
    Transform _Player = null;
    LayerMask _layer = default;
    LayerMask _enemylayer = default;
    bool isHacking = false;

    public override void Init()
    {
        _cost = 1;
        _damage = 25;
        _rangeType = "Point";
        _rangeScale = 3.0f;
        _rangeRange = 5.0f;

        _CastingTime = 0.7f;
        _effectTime = 0.7f;
    }

    public override void InitCard()
    {
        //Debug.Log($"{this.gameObject.name} is called");
        //Debug.Log($"마나 {_cost} 사용 ");
        //Debug.Log($"{_rangeScale}내 적 카드 사용 불가");
    }

    public override void UpdateInit()
    {
        if (_Ground != null && _enemylayer != default && !isHacking && _Player != default)
        {
            HackingOn(_Ground, _Player, _enemylayer);
            isHacking = true; // 호출 상태를 true로 변경
        }
    }
    

    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_HackingGrenade", Ground);

        _Ground = Ground;
        _Player = Player;
        _layer = layer;

        if (_layer == 1 << 6) { _enemylayer = 1 << 7; }
        if (_layer == 1 << 7) { _enemylayer = 1 << 6; }

        return _effectObject;
    }

    public void HackingOn(Transform Ground, Transform Player, LayerMask Enemylayer)
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
