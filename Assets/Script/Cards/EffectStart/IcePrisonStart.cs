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
    public override IEnumerator CardEffectInit(int userId, float time)
    {
        _pv = GetComponent<PhotonView>();
        player = Managers.game.RemoteTargetFinder(userId);
        effectTime = 3.0f;
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

    private void Update()
    {
        //_pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
	{
        startEffect += Time.deltaTime;

        if (startEffect > effectTime - 0.01f)
        {
            //_playerPV.RPC("photonStatSet", RpcTarget.All, "defensePower", -9999f);
            //_playerPV.RPC("photonStatSet", RpcTarget.All, "speed", _speed);

            //PhotonNetwork.Destroy(gameObject);
        }
    }
}
