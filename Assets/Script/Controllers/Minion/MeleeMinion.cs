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

    public override void Attack()
    {
        base.Attack();
        if (!PhotonNetwork.IsMasterClient) return;

        if (_targetEnemyTransform == null) return;

        //타겟이 적 Player일 시
        if (_targetEnemyTransform.tag == "PLAYER")
        {
            PhotonView targetPV = _targetEnemyTransform.GetComponent<PhotonView>();
            targetPV.RPC(
                "photonStatSet",
                RpcTarget.All,
                GetComponent<PhotonView>().ViewID,
                "receviedDamage",
                _oStats.basicAttackPower
            );

            //if (targetPV.GetComponent<PlayerStats>().nowHealth <= 0)
            //    Managers.game.killEvent(pv.ViewID, targetPV.ViewID);
        }
        else
        {
            PhotonView targetPV = _targetEnemyTransform.GetComponent<PhotonView>();
            targetPV.RPC(
                "photonStatSet",
                RpcTarget.All,
                "nowHealth",
                -_oStats.basicAttackPower
            );
        }
    }
}
