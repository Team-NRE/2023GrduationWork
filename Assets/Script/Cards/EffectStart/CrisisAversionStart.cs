using Define;
using Photon.Pun;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrisisAversionStart : BaseEffect
{
    PlayerStats pStats;
    
    GameObject player;
    GameObject effectOn;
    protected PhotonView _pv;


    float nowEffectTime = 0.01f;

    int effectTime = 7;
    int enemyLayer; 

    bool IsEffect = false;

# warning CrisisAversion은 스텟이 적용되지 않음
    [PunRPC]
    public override void CardEffectInit(int userId)
    {
        base.CardEffectInit(userId);
        // 
        pStats = player.GetComponent<PlayerStats>();
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
