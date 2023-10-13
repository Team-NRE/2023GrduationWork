using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiantCrystalStart : BaseEffect
{
    protected PhotonView _pv;
    protected PlayerStats _stats;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        _stats = player.GetComponent<PlayerStats>();
        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 0, 0);

        _stats.nowMana += 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
