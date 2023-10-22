using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Stat;


public class HackingGrenade2Start : BaseEffect
{
    float effectTime;
    float startEffect;

    protected PhotonView targetPV;

    PlayerStats targetStats;

    Transform ManaUI;

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        base.CardEffectInit(userId, targetId);
        
        targetStats = target.GetComponent<PlayerStats>();
        targetPV = target.GetComponent<PhotonView>();
        ManaUI = transform.Find("Canvas");

        transform.parent = target.transform;
        transform.localPosition = new Vector3(0, 2.3f, 0);

        ///스텟 적용 시간
        effectTime = 2.5f;
        startEffect = 0.01f;

        targetStats.nowState = "Debuff";
        targetStats.nowMana -= targetStats.nowMana;
        targetStats.manaRegen -= targetStats.manaRegen;
    }
    
    void Update()
    {
        startEffect += Time.deltaTime;

        ///스텟 적용 종료
        if (startEffect > effectTime - 0.01f || targetStats.nowState == "Health")
        {
            targetPV.RPC("photonStatSet", RpcTarget.All, "nowState", "Health");
            targetPV.RPC("photonStatSet", RpcTarget.All, "manaRegen", 0.25f);

            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        ManaUI.LookAt(ManaUI.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
    }
}
