
using UnityEngine;
using Stat;
using Photon.Pun;
using Photon.Realtime;

public class RangedBullet : MonoBehaviour
{
    //[SerializeField]
    //Transform _Target;
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
            Destroy(this.gameObject);
        }

        FollowTarget();
        HitDetection();
    }

    public void Init(int targetId)
    {
        _pv = GetComponent<PhotonView>();
        _target = GetRemotePlayer(targetId);
        _bulletSpeed = 5;
        _damage = 10;
        Debug.Log(_target.gameObject.name);
    }

    public void FollowTarget()
    {
        if (_target != null)
        {
            _TargetPos = _target.gameObject.transform.position;

            transform.position = Vector3.Slerp(transform.position, _TargetPos + Vector3.up, Time.deltaTime * _bulletSpeed);
            transform.LookAt(_TargetPos);
        }
        if (_target == null)
            Destroy(this.gameObject);
    }

    public void HitDetection()
    {
        Vector3 thisPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPos = new Vector3(_TargetPos.x, 0, _TargetPos.z);

        if (Vector3.Distance(thisPos, targetPos) <= 0.5f)
        {
            //타겟이 미니언, 타워일 시 
            if (_target.tag != "PLAYER")
            {
                ObjStats _Stats = _target.GetComponent<ObjStats>();
                _Stats.nowHealth -= _damage;
                _pv.RPC("RemoteLog", RpcTarget.All, _Stats.nowHealth.ToString());
            }

            //타겟이 적 Player일 시
            if (_target.tag == "PLAYER")
            {
                PlayerStats _Stats = _target.GetComponent<PlayerStats>();
                _Stats.nowHealth -= _damage;
                _pv.RPC("RemoteLog", RpcTarget.All, _Stats.nowHealth.ToString());
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
