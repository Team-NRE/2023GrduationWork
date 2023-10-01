using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class SpeedStart : BaseEffect
{
    PlayerStats _pStats;
    protected PhotonView _pv;

    float speed = default;
    float speed_Time = 0.01f;

    bool start = false;

    private void Start()
    {
        _pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        _pStats = player.GetComponent<PlayerStats>();
        this.gameObject.transform.parent = player.transform;
        speed = 1.5f;

        start = true;
    }

    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
	{
        if (start == true)
        {
            speed_Time += Time.deltaTime;

            if (speed_Time >= 4.99f)
            {
                _pStats.speed -= speed;
                start = false;
            }
        }
    }
}
