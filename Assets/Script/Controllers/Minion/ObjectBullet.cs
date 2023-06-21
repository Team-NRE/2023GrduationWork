/// ksPark
/// 
/// 원거리 공격의 총알 공격

using UnityEngine;
using Stat;

public class ObjectBullet : MonoBehaviour
{
    [SerializeField]
    Transform _Target;
    [SerializeField]
    Vector3 _TargetPos;

    [SerializeField]
    float _bulletSpeed;
    [SerializeField]
    float _damage;

    public void Update()
    {
        FollowTarget();
        HitDetection();
    }

    public void BulletSetting(Vector3 muzzle, Transform _target, float bulletSpeed, float damage)
    {
        transform.position = muzzle;
        _Target = _target;
        _bulletSpeed = bulletSpeed * 2f; // 공속 대비 2배 속도
        _damage = damage;
    }

    public void FollowTarget()
    {
        if (_Target == null)
            Destroy(this.gameObject);
        else
            _TargetPos = _Target.position;

        transform.position = Vector3.Slerp(transform.position, _TargetPos + Vector3.up, Time.deltaTime * _bulletSpeed);
        transform.LookAt(_TargetPos);
    }

    public void HitDetection()
    {
        Vector3 thisPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPos = new Vector3(_TargetPos.x, 0, _TargetPos.z);

        if (Vector3.Distance(thisPos, targetPos) <= 0.5f)
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
            
            Destroy(this.gameObject, 0.5f);
            this.enabled = false;
        }
    }
}