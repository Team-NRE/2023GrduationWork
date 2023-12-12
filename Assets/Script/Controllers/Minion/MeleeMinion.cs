/// ksPark
/// 
/// 근접 공격 미니언의 상세 코드 스크립트

using Stat;
using Define;
using Photon.Pun;

public class MeleeMinion : Minion
{
    public override void init()
    {
        base.init();
        _type = ObjectType.MeleeMinion;
    }

    /// 공격 함수
    public override void Attack()
    {
        base.Attack();
        // 예외 처리
        if (!PhotonNetwork.IsMasterClient) return; // 방장의 컴퓨터에서만 실행되도록 처리
        if (_targetEnemyTransform == null) return; // 타겟이 없을 경우 return

        // 타겟 PhotonView 가져오기
        PhotonView targetPV = _targetEnemyTransform.GetComponent<PhotonView>();

        //타겟이 적 Player일 시
        if (_targetEnemyTransform.tag == "PLAYER")
        {
            targetPV.RPC(
                "photonStatSet",
                RpcTarget.All,
                GetComponent<PhotonView>().ViewID, // 공격한 오브젝트
                "receviedDamage", // 체력 스텟에 처리
                _oStats.basicAttackPower  // 공격 데미지
            );
        }
        else // 타겟이 오브젝트 일 시
        {
            targetPV.RPC(
                "photonStatSet",
                RpcTarget.All,
                "nowHealth", // 체력 스텟에 처리
                -_oStats.basicAttackPower // 공격 데미지
            );
        }
    }
}
