using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class IcePrisonStart : BaseEffect
{
    float effectTime;
    float startEffect = 0.01f;
    protected PhotonView _pv;

    PlayerStats pStat;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        effectTime = 3.0f;
        pStat = player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
	{
        startEffect += Time.deltaTime;

        if (startEffect > effectTime - 0.01f)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
