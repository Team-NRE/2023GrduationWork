/// ksPark
/// 
/// 원거리 공격 미니언의 상세 코드 스크립트

using UnityEngine;
using Define;

using Photon.Pun;

public class RangeMinion : Minion
{
    [SerializeField]
    string bullet;

    public override void init() 
    {
        base.init();
        _type = ObjectType.RangeMinion;

        bullet = $"Prefabs/Projectile/{LayerMask.LayerToName(this.gameObject.layer)}MinionBullet";
    }

    // 공격 함수
    public override void Attack()
    {
        base.Attack();
        // 예외 처리
        if (!PhotonNetwork.IsMasterClient) return; // 방장의 컴퓨터에서만 실행되도록 처리
        if (_targetEnemyTransform == null) return; // 타겟이 없을 경우 return

        // 총알 오브젝트 생성
        GameObject nowBullet = PhotonNetwork.InstantiateRoomObject(
            bullet,
            this.transform.position,
            this.transform.rotation
        );

        // 총알 오브젝트 PhotonView 가져오기
        PhotonView bulletPv = nowBullet.GetComponent<PhotonView>();

        // 총알 오브젝트 초기화 Init
        bulletPv.RPC("BulletSetting",   // v2
            RpcTarget.All,
            GetComponent<PhotonView>().ViewID,
            _targetEnemyTransform.GetComponent<PhotonView>().ViewID,
            _oStats.attackSpeed,
            _oStats.basicAttackPower
        );
    }
}
