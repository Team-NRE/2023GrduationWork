/// ksPark
/// 
/// 원거리 공격의 총알 공격

using UnityEngine;
using Stat;

using Photon.Pun;

public class ObjectBullet : MonoBehaviourPun
{
    PhotonView _Shooter;
    Transform _Target;
    Vector3 _TargetPos;

    float _bulletSpeed;
    float _damage;

    public void Update()
    {
        FollowTarget();
        HitDetection();
    }

    [PunRPC]    // v2
    public void BulletSetting(int _shooter, int _target, float bulletSpeed, float damage)
    {
        // transform.position = muzzle;
        _Shooter = getTargetV2(_shooter).GetComponent<PhotonView>();
        _Target = getTargetV2(_target);   // **타겟 위치값 받기**
        _bulletSpeed = bulletSpeed * 2f; // 공속 대비 2배 속도
        _damage = damage;

        if (!PhotonNetwork.IsMasterClient) 
        {
            getTargetV2(_shooter)
                .GetComponent<ObjectController>()
                ._targetEnemyTransform = _Target;
        }
    }

    private Transform getTargetV2(int viewId)
    {
        return PhotonView.Find(viewId).transform;
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

        if (_Target == null) 
        {
            Destroy(this.gameObject);
        }
        else if (Vector3.Distance(thisPos, targetPos) <= 0.5f)
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

                if (_Stats.nowHealth <= 0)
                    Managers.game.killEvent(_Shooter.ViewID, _Target.GetComponent<PhotonView>().ViewID);
            }
            
            Destroy(this.gameObject, 0.5f);
            this.enabled = false;
        }
    }
}