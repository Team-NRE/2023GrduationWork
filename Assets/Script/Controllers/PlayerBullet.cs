
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
public class PlayerBullet : Poolable
{
    GameObject _Target;

    //외부 namespace Stat 참조
    public PlayerStats _pStats { get; set; }

    public void OnEnable()
    {
        _pStats = GameObject.FindWithTag("PLAYER").GetComponent<PlayerStats>();
    }

    public void Update()
    {
        Proj_Target_Init(_Target);
    }

    public override void Proj_Target_Init(GameObject _target)
    {
        _Target = _target;

        Vector3 target_Pos = new Vector3(_Target.transform.position.x, transform.position.y, _Target.transform.position.z);
        transform.position = Vector3.Slerp(transform.position, target_Pos, Time.deltaTime * _pStats.attackSpeed);

        if (Vector3.Distance(transform.position, target_Pos) <= 0.7f)
        {
            //타겟이 미니언, 타워일 시 
            if (_Target.tag != "PLAYER")
            {
                ObjStats _Stats = _Target.GetComponent<ObjStats>();
                _Stats.NowHealth -= _pStats._basicAttackPower;
            }

            //타겟이 적 Player일 시
            if (_Target.tag == "PLAYER")
            {
                PlayerStats _Stats = _Target.GetComponent<PlayerStats>();
                _Stats.nowHealth -= _pStats._basicAttackPower;
            }
            
            Managers.Pool.Push(this);
        }

    }

}
