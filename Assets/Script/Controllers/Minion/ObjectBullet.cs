/// ksPark
/// 
/// 원거리 공격의 총알 공격

using UnityEngine;
using Stat;

using Photon.Pun;

public class ObjectBullet : MonoBehaviourPun
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

    // [PunRPC]
    // public void BulletSetting(Vector3 muzzle, Vector3 _target, float bulletSpeed, float damage)
    // {
    //     // transform.position = muzzle;
    //     _Target = getTarget(_target);   // **타겟 위치값 받기**
    //     _bulletSpeed = bulletSpeed * 2f; // 공속 대비 2배 속도
    //     _damage = damage;
    //     tP = _target;
    // }

    [PunRPC]    // v2
    public void BulletSetting(int _shooter, int _target, float bulletSpeed, float damage)
    {
        // transform.position = muzzle;
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

    private Transform getTarget(Vector3 pos)
    {
        // Ray 생성, 타겟 위치 아래에서 위로 Ray 쏘기
        Ray ray = new Ray(
            pos + Vector3.down, 
            Vector3.up * 2
            );

        RaycastHit hitData;

        Debug.DrawRay(pos, Vector3.up);

        Physics.Raycast(
            ray, 
            out hitData, 
            2.0f, 
            (1 << LayerMask.NameToLayer("Cyborg")) | (1 << LayerMask.NameToLayer("Human"))
        );

        return hitData.transform;
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
            }
            
            Destroy(this.gameObject, 0.5f);
            this.enabled = false;
        }
    }
}