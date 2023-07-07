
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class RangedBullet : MonoBehaviour
{
    private Transform _Target;
    private float _BulletSpeed;
    private float _BulletDamage;

    //외부 namespace Stat 참조
    public PlayerStats _pStats { get; set; }

	public void Start()
	{
		
	}

	public void OnEnable()
    {
        GameObject _player = GameObject.FindWithTag("PLAYER");

        _pStats = _player.GetComponent<PlayerStats>();

        //GetClickedTarget(transform.position, _Target, _BulletSpeed, _BulletDamage);
    }



    public void Update()
    {
        Bullet_shoot();
    }

    public void GetClickedTarget(Vector3 _shooter, Transform _target, float _bulletSpeed, float _damage)
    {
        transform.position = _shooter;
        _Target = _target;
        _BulletSpeed = _bulletSpeed;
        _BulletDamage = _damage;
    }

    void Bullet_shoot()
    {
        Vector3 target_Pos = new Vector3(_Target.position.x, transform.position.y, _Target.position.z);
        transform.position = Vector3.Lerp(transform.position, target_Pos, Time.deltaTime * _BulletSpeed);

        if (Vector3.Distance(transform.position, target_Pos) <= 0.7f)
        {
            //타겟이 미니언, 타워일 시 
            if (_Target.tag != "PLAYER")
            {
                ObjStats _Stats = _Target.GetComponent<ObjStats>();
                _Stats.nowHealth -= _BulletDamage;
                Debug.Log($"{_Stats.nowHealth}");
            }

            //타겟이 적 Player일 시
            if (_Target.tag == "PLAYER")
            {
                PlayerStats _Stats = _Target.GetComponent<PlayerStats>();
                _Stats.nowHealth -= _BulletDamage;
                Debug.Log($"{_Stats.nowHealth}");
            }

            //Managers.Pool.Push(this);
            PhotonNetwork.Destroy(this.gameObject);
        }

    }

}
