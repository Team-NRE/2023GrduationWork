using Photon.Pun;
using Stat;
using UnityEngine;

public class ArmorStart : BaseEffect
{
    PlayerStats _pStat;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        ///초기화
        base.CardEffectInit(userId);
        _pStat = player.GetComponent<PlayerStats>();

        ///effect 위치
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.8f, 0);

        ///스텟 적용
        _pStat.defensePower += 0.1f;
    }
}
