using Define;
using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrisisAversionStart : BaseEffect
{
    PlayerStats pStats;
    
    GameObject effectOn;
    protected PhotonView _pv;


    float nowEffectTime = 0.01f;

    int effectTime = 7;
    int enemyLayer; 

    bool IsEffect = false;

    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);

        enemyLayer = player.GetComponent<PlayerStats>().enemyArea;
        pStats = player.GetComponent<PlayerStats>();
        this.gameObject.transform.parent = player.transform;
        this.gameObject.transform.localPosition = new Vector3(0, 0.3f, 0);
    }


    public void Update()
    {
        _pv.RPC("RpcUpdate", RpcTarget.All);
    }

    [PunRPC]
    public void RpcUpdate()
	{
        // ���� �ǰ� 1�� �Ǿ��ٸ�....
        // �� ����ü�� �°� �ǰ� 1���Ϸ� �������� �ȴٸ� �ߵ�....
        // ���� �������� �̿ϼ� 
        if (pStats.nowHealth <= 0)
        {
            IsEffect = true;
            effectOn.SetActive(IsEffect);
        }

        if (IsEffect == true)
        {
            //�� 1�� ����
            pStats.nowHealth = 1;

            nowEffectTime += Time.deltaTime;

            if (nowEffectTime >= effectTime)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
