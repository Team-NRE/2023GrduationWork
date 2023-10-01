
using UnityEngine;
using Stat;
using Photon.Pun;
using Photon.Realtime;
using static UnityEngine.GraphicsBuffer;

public class RangedBullet : MonoBehaviour
{
    GameObject _player;
    GameObject _target;
    [SerializeField]
    Vector3 _TargetPos;

    [SerializeField]
    float _bulletSpeed;
    [SerializeField]
    float _damage;

    PhotonView _pv;

    public void Update()
    {
        if (_target == null)
        {
            Debug.Log("Ranged Bullet Target Null");
            Destroy(this.gameObject);
        }

        //_pv.RPC("FollowTarget", RpcTarget.All);
        //_pv.RPC("HitDetection", RpcTarget.All);
        FollowTarget();
        HitDetection();
    }

    [PunRPC]
    public void Init(int playerId,int targetId)
    {
        _pv = GetComponent<PhotonView>();
        _player = GetRemotePlayer(playerId);
        _target = GetRemotePlayer(targetId);
        _bulletSpeed = 5;
        _damage = _player.GetComponent<PlayerStats>().basicAttackPower;
    }

    public void FollowTarget()
    {
        if (_target == null)
            Destroy(this.gameObject);

        if (_target != null)
        {
            _TargetPos = _target.gameObject.transform.position;

            transform.position = Vector3.Slerp(transform.position, _TargetPos + Vector3.up, Time.deltaTime * _bulletSpeed);
            transform.LookAt(_TargetPos);
        }
    }

    public void HitDetection()
    {
        Vector3 thisPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPos = new Vector3(_TargetPos.x, 0, _TargetPos.z);

        if (Vector3.Distance(thisPos, targetPos) <= 0.5f)
        {
            PhotonView _playerPV = _player.GetComponent<PhotonView>();
            PhotonView _targetPV = _target.GetComponent<PhotonView>();
            //타겟이 미니언, 타워일 시 
            if (_target.tag != "PLAYER")
            {
                _targetPV.RPC("photonStatSet", RpcTarget.All, "nowHealth", -_damage);

                if (_target.GetComponent<ObjStats>().nowHealth <= 0)
                {
                    BaseCard._lockTarget = null;
                }
            }

            //타겟이 적 Player일 시
            if (_target.tag == "PLAYER")
            {
                _targetPV.RPC("photonStatSet", RpcTarget.All, _player.GetComponent<PhotonView>().ViewID, "receviedDamage", _damage);

                if(_target.GetComponent<PlayerStats>().nowHealth <= 0)
                {
                    BaseCard._lockTarget = null;
                }
            }

            Destroy(this.gameObject, 0.5f);
            this.enabled = false;
        }
    }

    protected int GetRemotePlayerId(GameObject target)
    {
        int remoteId = target.GetComponent<PhotonView>().ViewID;
        return remoteId;
    }

    protected GameObject GetRemotePlayer(int remoteId)
    {
        GameObject target = PhotonView.Find(remoteId)?.gameObject;
        return target;
    }

    protected Vector3 GetRemoteVector(int remoteId)
    {
        Vector3 targetVector = GetRemotePlayer(remoteId).transform.position;
        return targetVector;
    }

    [PunRPC]
    private void RemoteLog(string log)
    {
        Debug.Log(log);
    }
}
