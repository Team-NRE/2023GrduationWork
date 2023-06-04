/// ksPark
/// 
/// 원거리 공격의 총알 공격

using UnityEngine;
using Stat;
using Photon.Pun;

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
        _bulletSpeed = bulletSpeed * 2; // 공속 대비 3배 속도
        _damage = damage;
    }

    public void FollowTarget()
    {
        if (_Target == null) 
        {
            Destroy(this.gameObject);
            PhotonNetwork.Destroy(this.gameObject);
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
            
            Destroy(this.gameObject);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}