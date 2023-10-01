using Photon.Pun;
using Stat;
using UnityEngine;

public class ArmorStart : BaseEffect
{
    PlayerStats _pStats;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        _pStats = player.GetComponent<PlayerStats>();
        transform.parent = player.transform;
        transform.localPosition = new Vector3(0, 0.8f, 0);

        _pStats.defensePower += 0.5f;
    }
}
