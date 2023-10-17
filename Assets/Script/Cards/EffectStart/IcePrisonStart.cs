using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class IcePrisonStart : BaseEffect
{
    PhotonView _pv;
    PlayerStats pStat;

    [PunRPC]
    public override IEnumerator CardEffectInit(int userId, float time)
    {
        _pv = GetComponent<PhotonView>();
        player = Managers.game.RemoteTargetFinder(userId);
        pStat = player.GetComponent<PlayerStats>();
        float originalSpeed = pStat.GetComponent<PlayerStats>().speed;

        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 0.3f, 0);

        pStat.defensePower += 9999f;
        pStat.speed = 0.0f;
        yield return new WaitForSeconds(time);
        pStat.defensePower -= 9999f;
        pStat.speed = originalSpeed;
    }
}
