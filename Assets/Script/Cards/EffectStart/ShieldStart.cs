using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class ShieldStart : BaseEffect
{
    float effectTime;
    float startEffect = 0.01f;

    protected PhotonView _pv;
    protected PhotonView _playerPV;

    PlayerStats pStat;

    float shieldValue = default;
    float shieldRatioPerHealth = 0.2f;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);

        pStat = player.GetComponent<PlayerStats>();
        _playerPV = player.GetComponent<PhotonView>();

        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 1.12f, 0);

        effectTime = 3.0f;
        shieldValue = pStat.maxHealth * shieldRatioPerHealth;

        _playerPV.RPC("photonStatSet", RpcTarget.All, "firstShield", shieldValue);
        _playerPV.RPC("photonStatSet", RpcTarget.All, "shield", shieldValue);

        //Debug.Log($"초기 방어막 생성 : {shieldValue} , {pStat.firstShield}");
        //Debug.Log($"중첩 방어막 생성 : {pStat.shield}");
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        if (startEffect > effectTime - 0.01f)
        {
            //초기 쉴드와 지금의 쉴드가 같지 않다면
            if (pStat.firstShield != pStat.shield)
            {
                pStat.shield = 0;
                _playerPV.RPC("photonStatSet", RpcTarget.All, "firstShield", -shieldValue);
                _playerPV.RPC("photonStatSet", RpcTarget.All, "shield", pStat.firstShield);

                //Debug.Log($"초기 방어막 : {pStat.firstShield}");
                //Debug.Log($"지금 방어막 : {pStat.shield}");

                Destroy(gameObject);

                return;
            }

            //초기 쉴드와 지금의 쉴드가 같다면
            if (pStat.firstShield == pStat.shield)
            {
                _playerPV.RPC("photonStatSet", RpcTarget.All, "firstShield", -shieldValue);
                _playerPV.RPC("photonStatSet", RpcTarget.All, "shield", -shieldValue);

                //Debug.Log($"초기 방어막 : {pStat.firstShield}");
                //Debug.Log($"지금 방어막 : {pStat.shield}");

                Destroy(gameObject);

                return;
            }
        }
    }
}
