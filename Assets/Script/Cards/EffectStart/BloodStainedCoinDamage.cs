using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStainedCoinDamage : BaseEffect
{
    public PhotonView _pv;

    float effectTime;
    float startEffect;

    PlayerStats target_pStat;
    ObjStats target_oStat;

    [PunRPC]
    public override void CardEffectInit(int userId, int remoteTargetId)
    {
        //초기화
        base.CardEffectInit(userId, remoteTargetId);
        _pv = GetComponent<PhotonView>();

        //effect 위치
        transform.parent = target.transform;
        transform.localPosition = new Vector3(0, 0.8f, 0);
        
        //데미지 피해
        damage = 10.0f;
        if (target.gameObject.CompareTag("PLAYER"))
        {
            target_pStat = target.GetComponent<PlayerStats>();
            target_pStat.receviedDamage = (playerId, damage + (pStat.basicAttackPower * 1.0f));
        }
        if(!target.gameObject.CompareTag("PLAYER"))
        {
            target_oStat = target.GetComponent<ObjStats>();
            target_oStat.nowHealth -= damage + (pStat.basicAttackPower * 1.0f);
        }

        ///스텟 적용 시간
        effectTime = 2.0f;
        startEffect = 0.01f;
    }

    public void Update()
    {
        startEffect += Time.deltaTime;

        ///스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            //effect 적용
            Destroy(this.gameObject);

            return;
        }
    }
}
