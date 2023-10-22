
using UnityEngine;
using Stat;
using Photon.Pun;
using Photon.Realtime;
using static UnityEngine.GraphicsBuffer;

public class RangedBullet : MonoBehaviour
{
    GameObject _player;
    GameObject _target;
    
    Vector3 _TargetPos;

    float _bulletSpeed;
    float _damage;

    int _playerId;
    int _targetId;

    PhotonView _pv;

    PlayerStats _pStats;


    [PunRPC]
    public void Init(int playerId, int targetId)
    {
        //ViewId로 오브젝트 찾기
        _player = Managers.game.RemoteTargetFinder(playerId);
        _target = Managers.game.RemoteTargetFinder(targetId);

        //초기화
        _pv = GetComponent<PhotonView>();
        _pStats = _player.GetComponent<PlayerStats>();

        //ViewId 저장
        _playerId = playerId;
        _targetId = targetId;

        //bullet 정보 저장
        _bulletSpeed = 5f;
        _damage = _pStats.basicAttackPower;
    }


    public void Update()
    {
        //target null일 때 종료 
        if (_target == null)
        {
            Destroy(this.gameObject);

            return;
        }
        //target null이 아닐때 추적
        if (_target != null)
        {
            FollowTarget();
        }
    }

    public void FollowTarget()
    {
        _TargetPos = _target.transform.position;

        transform.position = Vector3.Slerp(transform.position, _TargetPos + Vector3.up, Time.deltaTime * _bulletSpeed);
        transform.LookAt(_TargetPos);

        Vector3 thisPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPos = new Vector3(_TargetPos.x, 0, _TargetPos.z);

        if (Vector3.Distance(thisPos, targetPos) <= 0.6f)
        {
            _pv.RPC("ApplyDamage", RpcTarget.All);
        }
    }

    [PunRPC]
    protected void ApplyDamage()
    {
        //Player
        if (_target.CompareTag("PLAYER"))
        {
            //초기화
            PlayerStats target_pStats = _target.GetComponent<PlayerStats>();
            
            //데미지 피해
            target_pStats.receviedDamage = (_playerId, _pStats.basicAttackPower);

            //target 사망 시
            if (target_pStats.nowHealth <= 0)
            {
                BaseCard._lockTarget = null;
            }

            // 해당 gameobject 파괴
            Destroy(gameObject);
            this.enabled = false;

            return;
        }
        //object
        if(!_target.CompareTag("PLAYER"))
        {
            //초기화
            ObjStats target_oStats = _target.GetComponent<ObjStats>();

            //데미지 피해
            target_oStats.nowHealth -= _pStats.basicAttackPower;

            //target 사망 시
            if (target_oStats.nowHealth <= 0)
            {
                BaseCard._lockTarget = null;
            }

            // 해당 gameobject 파괴
            Destroy(gameObject);
            this.enabled = false;

            return;
        }
    }
}
