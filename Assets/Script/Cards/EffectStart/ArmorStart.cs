using Photon.Pun;
using Stat;
using UnityEngine;

public class ArmorStart : BaseEffect
{
    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        //초기화
        base.CardEffectInit(userId);

        //effect 위치
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.8f, 0);

        //RPC 적용
        playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", 0.1f);
    }
}
