using UnityEngine;
using Stat;

public class SpearStart : MonoBehaviour
{
    int _Enemylayer = default;
    float _bulletSpeed;
    float _damage;

    Transform _player;
    Rigidbody _rigid;

    ObjStats _Stats;
    PlayerStats _pStats;
    PlayerStats _EnemyStats;

    public void Setting(Transform Player, LayerMask _enemylayer, float bulletSpeed, float damage)
    {
        _player = Player;
        _Enemylayer = _enemylayer;
        _bulletSpeed = bulletSpeed * 2f; // 공속 대비 2배 속도
        _damage = damage;
    }

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        FollowTarget();
        //HitDetection();
    }

    public void FollowTarget()
    {
        Vector3 SpearDirection = _player.forward;
        _rigid.AddForce(SpearDirection * _bulletSpeed);
        transform.Rotate(new Vector3(-90, 0, Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _Enemylayer)
        {
            Debug.Log(other.gameObject.name);

            //타겟이 미니언, 타워일 시 
            if (other.gameObject.tag != "PLAYER")
            {
                _Stats = other.gameObject.GetComponent<ObjStats>();
                _pStats = _player.gameObject.GetComponent<PlayerStats>();

                _Stats.nowHealth -= (_damage + (_pStats.basicAttackPower * 0.7f));
            }

            //타겟이 적 _player일 시
            if (other.gameObject.tag == "PLAYER")
            {
                _EnemyStats = other.gameObject.GetComponent<PlayerStats>();
                _pStats = _player.gameObject.GetComponent<PlayerStats>();

                _EnemyStats.nowHealth -= (_damage + (_pStats.basicAttackPower * 0.7f));
            }

            Destroy(this.gameObject);
        }
    }
    /*
    public void HitDetection()
    {
        Vector3 thisPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPos = new Vector3(_TargetPos.x, 0, _TargetPos.z);

        if (Vector3.Distance(thisPos, targetPos) <= 0.5f)
        {
            //타겟이 미니언, 타워일 시 
            if (_Target.tag != "_player")
            {
                ObjStats _Stats = _Target.GetComponent<ObjStats>();
                _Stats.nowHealth -= _damage;
            }

            //타겟이 적 _player일 시
            if (_Target.tag == "_player")
            {
                _playerStats _Stats = _Target.GetComponent<_playerStats>();
                _Stats.nowHealth -= _damage;
            }
            
            Destroy(this.gameObject, 0.5f);
            this.enabled = false;
        }
    }*/
}
