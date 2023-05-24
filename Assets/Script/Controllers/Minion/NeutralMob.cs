/// ksPark
/// 
/// 중립 몹의 상세 코드 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class NeutralMob : ObjectController
{
    public override void init()
    {
        base.init();
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void Death()
    {
        base.Death();
    }

    protected override void UpdateObjectAction()
    {
        if (_targetEnemyTransform != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_targetEnemyTransform.position - transform.position), Time.deltaTime * 2.0f);
            _action = ObjectAction.Attack;
        }
        else 
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Time.deltaTime);
            _action = ObjectAction.Idle;
            
        }
    }
}
