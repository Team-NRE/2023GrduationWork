/// ksPark
/// 
/// 근접 공격 미니언의 상세 코드 스크립트

using UnityEngine;
using UnityEngine.AI;
using Define;
using Stat;

public class MeleeMinion : Minion
{
    public override void Attack()
    {
        //타겟이 미니언, 타워일 시 
        if (_targetEnemyTransform.tag != "PLAYER")
        {
            ObjStats _Stats = _targetEnemyTransform.GetComponent<ObjStats>();
            _Stats.nowHealth -= _oStats.basicAttackPower;
        }

        //타겟이 적 Player일 시
        if (_targetEnemyTransform.tag == "PLAYER")
        {
            PlayerStats _Stats = _targetEnemyTransform.GetComponent<PlayerStats>();
            _Stats.nowHealth -= _oStats.basicAttackPower;
        }
    }
}