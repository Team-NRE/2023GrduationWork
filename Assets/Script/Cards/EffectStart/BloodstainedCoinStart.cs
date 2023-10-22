using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;
using static UnityEngine.GraphicsBuffer;
using Photon.Pun;

public class BloodstainedCoinStart : BaseEffect
{
    PhotonView _pv;

    [PunRPC]
    public override void CardEffectInit(int userId, int targetId)
    {
        //초기화
        base.CardEffectInit(userId, targetId);
        _pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
    {
        Vector3 thisPos = transform.position;
        Vector3 targetPos = target.transform.position;

        transform.position = Vector3.Slerp(transform.position, targetPos + Vector3.up, Time.deltaTime * 2.0f);

        if (Vector3.Distance(thisPos, targetPos) <= 1.5f)
        {
            GameObject _effectObject = PhotonNetwork.Instantiate("Prefabs/Particle/Effect_BloodstainedCoin", targetPos, Quaternion.identity);
            _effectObject.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, playerId, targetId);
            Destroy(this.gameObject);

            return;
        }
    }
}
