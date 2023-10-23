using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Stat;


public class HackingGrenade2Start : BaseEffect
{
    Transform ManaUI;

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        //초기화
        base.CardEffectInit(userId, targetId);
        target_pStat = target.GetComponent<PlayerStats>();
        ManaUI = transform.Find("Canvas");

        //effect 위치
        transform.parent = target.transform;
        transform.localPosition = new Vector3(0, 2.3f, 0);

        //스텟 적용 시간
        effectTime = 2.5f;
        startEffect = 0.01f;

        //스텟 적용 
        manaRegenValue = 0.25f;

        //RPC 적용
        targetPV.RPC("photonStatSet", RpcTarget.All, "nowState", "Debuff");
        targetPV.RPC("photonStatSet", RpcTarget.All, "nowMana", -target_pStat.nowMana);
        targetPV.RPC("photonStatSet", RpcTarget.All, "manaRegen", -manaRegenValue);
    }
    
    void Update()
    {
        startEffect += Time.deltaTime;

        //스텟 적용 종료
        if (startEffect > effectTime - 0.01f || target_pStat.nowState == "Health")
        {
            targetPV.RPC("photonStatSet", RpcTarget.All, "nowState", "Health");
            targetPV.RPC("photonStatSet", RpcTarget.All, "manaRegen", manaRegenValue);

            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        ManaUI.LookAt(ManaUI.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
    }
}
