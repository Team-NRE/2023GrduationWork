using System.Collections;
using UnityEngine;
using Photon.Pun;
using Stat;

public class AmuletOfSteelStart : BaseEffect
{
    float effectTime;
    float startEffect;

    protected PhotonView _pv;
    protected PhotonView _playerPV;

    new PlayerStats pStat;

    float shieldValue = default;
    float shieldRatioPerHealth;


    void Start()
    {
        ///초기화
        player = transform.parent.gameObject; 
        pStat = player.GetComponent<PlayerStats>();
        _playerPV = player.GetComponent<PhotonView>();

        ///스텟 적용 시간
        effectTime = 5.0f;
        startEffect = 0.01f;
        shieldRatioPerHealth = 0.4f;

        ///스텟 적용 (방어막)
        shieldValue = pStat.maxHealth * shieldRatioPerHealth;
        ///현재 카드의 Max 방어막
        _playerPV.RPC("photonStatSet", RpcTarget.All, "firstShield", shieldValue);
        ///현재 Player의 방어막
        _playerPV.RPC("photonStatSet", RpcTarget.All, "shield", shieldValue);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        ///스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            ///방어막 카드의 Max 방어막 != 현재 Player의 방어막
            if (pStat.firstShield != pStat.shield)
            {
                ///방어막 수치 잠시 초기화
                pStat.shield = 0;
                ///방어막 카드의 총 Max 방어막 - 현재 카드의 Max 방어막
                _playerPV.RPC("photonStatSet", RpcTarget.All, "firstShield", -shieldValue);
                ///현재 Player의 방어막 += 남은 카드의 Max 방어막
                _playerPV.RPC("photonStatSet", RpcTarget.All, "shield", pStat.firstShield);

                //현재 카드 삭제
                Destroy(gameObject);

                return;
            }

            ///방어막 카드의 Max 방어막 == 현재 Player의 방어막
            if (pStat.firstShield == pStat.shield)
            {
                ///방어막 카드의 총 Max 방어막 - 현재 카드의 Max 방어막
                _playerPV.RPC("photonStatSet", RpcTarget.All, "firstShield", -shieldValue);
                ///현재 Player의 방어막 - 현재 카드의 Max 방어막
                _playerPV.RPC("photonStatSet", RpcTarget.All, "shield", -shieldValue);

                ///현재 카드 삭제
                Destroy(gameObject);

                return;
            }
        }
    }
}
