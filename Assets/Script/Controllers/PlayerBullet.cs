
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
public class PlayerBullet : Poolable
{
    private Vector3 _Shooter;
    private Transform _Target;
    private float _BulletSpeed;
    private float _BulletDamage;
    


    //외부 namespace Stat 참조
    public PlayerStats _pStats { get; set; }

    public void OnEnable()
    {
        GameObject _player = GameObject.Find("PlayerController");

        _Shooter = _player.transform.position;
        _pStats = _player.GetComponent<PlayerStats>();
    }

    public void Update()
    {
        Proj_Target_Init(_Shooter, _Target, _BulletSpeed, _BulletDamage);
    }

    public override void Proj_Target_Init(Vector3 _shooter, Transform _target, float _bulletSpeed, float _damage)
    {
        _Shooter = _shooter;
        _Target = _target;
        _BulletSpeed = _bulletSpeed;
        _BulletDamage = _damage;


        Vector3 target_Pos = new Vector3(_Target.position.x, transform.position.y, _Target.position.z);
        transform.position = Vector3.Slerp(transform.position, target_Pos, Time.deltaTime * _bulletSpeed);

        if (Vector3.Distance(transform.position, target_Pos) <= 0.7f)
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
