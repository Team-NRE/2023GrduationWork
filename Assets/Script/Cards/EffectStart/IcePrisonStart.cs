using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using Photon.Pun;

public class IcePrisonStart : MonoBehaviour
{
    float effectTime;
    float startEffect = 0.01f;
    PhotonView _pv;
    GameObject player = null;

    PlayerStats pStat;

    public void StartIcePrison(int _player, float _effectTime)
    {
        effectTime = _effectTime;

        //player = GameObject.Find(_player);
        player = Managers.game.RemoteTargetFinder(_player);
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
            Destroy(gameObject);
        }
    }
}
