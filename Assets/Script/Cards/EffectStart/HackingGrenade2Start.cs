using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Stat;


public class HackingGrenade2Start : BaseEffect
{
    float effectTime;
    float startEffect = 0.01f;

    protected PhotonView _pv;
    protected PhotonView _targetPV;

    PlayerStats pStat;

    void Start()
    {
        ///초기화
        player = transform.parent.gameObject;
        pStat = player.GetComponent<PlayerStats>();
        _targetPV = player.GetComponent<PhotonView>();

        ///스텟 적용 시간
        effectTime = 2.5f;

        _targetPV.RPC("photonStatSet", RpcTarget.All, "nowState", "Debuff");
        _targetPV.RPC("photonStatSet", RpcTarget.All, "nowMana", -pStat.nowMana);
        _targetPV.RPC("photonStatSet", RpcTarget.All, "manaRegen", 0);
    }

    // Update is called once per frame
    void Update()
    {
        startEffect += Time.deltaTime;

        ///스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            _targetPV.RPC("photonStatSet", RpcTarget.All, "nowState", "Health");
            _targetPV.RPC("photonStatSet", RpcTarget.All, "manaRegen", 0.25);
        }
    }
}
