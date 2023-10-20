using Photon.Pun;
using Stat;
using System.Collections;
using UnityEngine;

public class WingsOfTheBattlefieldStart : BaseEffect
{
    float effectTime;
    float startEffect = 0.01f;

    protected PhotonView _pv;
    protected PhotonView _playerPV;

    PlayerStats pStat;

    float shieldValue = default;
    float shieldRatioPerHealth = 0.4f;


    void Start()
    {
        ///초기화
        player = transform.parent.gameObject;
        pStat = player.GetComponent<PlayerStats>();
        _playerPV = player.GetComponent<PhotonView>();

        ///스텟 적용 시간
        effectTime = 4.0f;
        _speed = 2.0f;

        ///방어막 카드의 총 Max 방어막 - 현재 카드의 Max 방어막
        _playerPV.RPC("photonStatSet", RpcTarget.All, "speed", _speed);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        ///스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            ///방어막 카드의 총 Max 방어막 - 현재 카드의 Max 방어막
            _playerPV.RPC("photonStatSet", RpcTarget.All, "speed", -_speed);

            //현재 카드 삭제
            Destroy(gameObject);

            return;
        }
    }
}
