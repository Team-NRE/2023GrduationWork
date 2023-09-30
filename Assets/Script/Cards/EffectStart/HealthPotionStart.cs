using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionStart : BaseEffect
{
    protected PhotonView _pv;
    protected PlayerStats _stats;

    private void Start()
    {
        _pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        _stats = player.GetComponent<PlayerStats>();

        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 0, 0);

        _stats.nowHealth += 130;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �� ī�忡�� update�� �ʿ��� RPC�� ����
    [PunRPC]
    public void RpcUpdate()
    {

    }
}
