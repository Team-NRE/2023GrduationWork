using UnityEngine;
using Stat;
using Photon.Pun;

public class SpearStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId, Quaternion effectRotation)
    {
        //초기화
        base.CardEffectInit(userId, effectRotation);
        effectPV = GetComponent<PhotonView>();
        
        //Layer 초기화
        enemyLayer = pStat.enemyArea;
        
        //effect 위치 
        transform.rotation = effectRot;
        
        //스텟 적용
        projectileSpeedValue = 30.0f;
        powerValue = (50f, 0.7f);
        damageValue = powerValue.Item1 + (pStat.basicAttackPower * powerValue.Item2);
    }

    public void OnTriggerEnter(Collider other)
    {
        //Human & Cyborg & Neutral 매개체 외 return
        if (other.gameObject.layer != (int)Define.Layer.Human && other.gameObject.layer != (int)Define.Layer.Cyborg
                && other.gameObject.layer != (int)Define.Layer.Neutral)
            return;

        //접근한 Collider의 ViewId 찾기 
        int otherId = Managers.game.RemoteColliderId(other);

        //해당 ViewId가 default면 return
        if (otherId == default)
            return;

        //RPC 적용
        effectPV.RPC("RpcTrigger", RpcTarget.All, otherId);
    }

    [PunRPC]
    public void RpcTrigger(int otherId)
    {
        //Trigger로 선별된 ViewId의 게임오브젝트 초기화
        GameObject other = Managers.game.RemoteTargetFinder(otherId);

        //오브젝트가 없다면 return
        if (other == null)
            return;

        //해당 오브젝트가 다른 팀이라면
        if (other.layer == enemyLayer || other.layer == (int)Define.Layer.Neutral)
        {
            //타겟이 미니언, 타워일 시 
            if (!other.CompareTag("PLAYER"))
            {
                ObjStats target_oStats = other.GetComponent<ObjStats>();

                target_oStats.nowHealth -= damageValue;
            }

            //타겟이 Player일 시
            if (other.CompareTag("PLAYER"))
            {
                PlayerStats target_pStats = other.GetComponent<PlayerStats>();

                target_pStats.receviedDamage = (playerId, damageValue);
            }
        }
    }

    public void Update()
    {
        Vector3 SpearDirection = player.transform.forward;
        GetComponent<Rigidbody>().AddForce(SpearDirection * projectileSpeedValue);
        transform.Rotate(new Vector3(-90, 0, Time.deltaTime));
    }

    //[PunRPC]
    //public void FollowTarget(int userId) 
    //{
    //    Vector3 SpearDirection = player.transform.forward;
    //    GetComponent<Rigidbody>().AddForce(SpearDirection * bulletSpeed);
    //    transform.Rotate(new Vector3(-90, 0, Time.deltaTime));
    //}

}
