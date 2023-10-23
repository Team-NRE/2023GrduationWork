using Photon.Pun;
using Stat;
using System.Collections;
using UnityEngine;

public class WingsOfTheBattlefieldStart : BaseEffect
{
    void Start()
    {
        //초기화
        player = transform.parent.gameObject;
        pStat = player.GetComponent<PlayerStats>();
        playerPV = player.GetComponent<PhotonView>();

        //스텟 적용 시간 
        effectTime = 4.0f;
        startEffect = 0.01f;

        //스텟 적용
        speedValue = 2.0f;

        //RPC 적용
        playerPV.RPC("photonStatSet", RpcTarget.All, "speed", speedValue);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        //스텟 적용 종료
        if (startEffect > effectTime - 0.01f)
        {
            playerPV.RPC("photonStatSet", RpcTarget.All, "speed", -speedValue);

            Destroy(gameObject);

            return;
        }
    }
}
