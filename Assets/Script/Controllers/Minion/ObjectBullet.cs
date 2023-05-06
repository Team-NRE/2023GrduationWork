/// ksPark
/// 
/// 원거리 공격의 총알 공격

using UnityEngine;
using Stat;

public class ObjectBullet : Poolable
{
    Transform _Target;
    Vector3 _TargetPos;

    float _bulletSpeed;
    float _damage;

    public void Update()
    {
        FollowTarget();
        HitDetection();
    }

    public override void Proj_Target_Init(Vector3 muzzle, Transform _target, float bulletSpeed, float damage)
    {
        transform.position = muzzle;
        _Target = _target;
        _bulletSpeed = bulletSpeed;
        _damage = damage;
    }

    public void FollowTarget()
    {
        if (_Target == null) 
        {
            Managers.Pool.Push(this);
            return;
        }

        _TargetPos = _Target.position;

        transform.position = Vector3.Slerp(transform.position, _TargetPos, Time.deltaTime * _bulletSpeed);
        transform.LookAt(_TargetPos);
    }

    public void HitDetection()
    {
        if (Vector3.Distance(transform.position, _TargetPos) <= 0.7f)
        {
            //타겟이 미니언, 타워일 시 
            if (_Target.tag != "PLAYER")
            {
                ObjStats _Stats = _Target.GetComponent<ObjStats>();
                _Stats.nowHealth -= _damage;
            }

            //타겟이 적 Player일 시
            if (_Target.tag == "PLAYER")
            {
                PlayerStats _Stats = _Target.GetComponent<PlayerStats>();
                _Stats.nowHealth -= _damage;
            }
            
            Managers.Pool.Push(this);
        }
    }
}