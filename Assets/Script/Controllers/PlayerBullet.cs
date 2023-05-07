
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
public class PlayerBullet : Poolable
{
    private Transform _Target;
    private float _BulletSpeed;
    private float _BulletDamage;
    


    //외부 namespace Stat 참조
    public PlayerStats _pStats { get; set; }

    public void OnEnable()
    {
        GameObject _player = GameObject.Find("PlayerController");

        _pStats = _player.GetComponent<PlayerStats>();

        Proj_Target_Init(transform.position, _Target, _BulletSpeed, _BulletDamage);
    }



    public void Update()
    {
        Bullet_shoot();
    }

    public override void Proj_Target_Init(Vector3 _shooter, Transform _target, float _bulletSpeed, float _damage)
    {
        transform.position = _shooter;
        _Target = _target;
        _BulletSpeed = _bulletSpeed;
        _BulletDamage = _damage;
    }

    void Bullet_shoot()
    {
        Vector3 target_Pos = new Vector3(_Target.position.x, transform.position.y, _Target.position.z);
        transform.position = Vector3.Slerp(transform.position, target_Pos, Time.deltaTime * _BulletSpeed);

        if (Vector3.Distance(transform.position, target_Pos) <= 0.7f)
        {
            //타겟이 미니언, 타워일 시 
            if (_Target.tag != "Player")
            {
                ObjStats _Stats = _Target.GetComponent<ObjStats>();
                _Stats.nowHealth -= _BulletDamage;
            }

            //타겟이 적 Player일 시
            if (_Target.tag == "Player")
            {
                PlayerStats _Stats = _Target.GetComponent<PlayerStats>();
                _Stats.nowHealth -= _BulletDamage;
            }

            Managers.Pool.Push(this);
        }

    }

}
