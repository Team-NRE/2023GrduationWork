using Define;
using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrisisAversionStart : MonoBehaviour
{
    PlayerStats pStats;
    
    GameObject player;
    GameObject effectOn;

    float nowEffectTime = 0.01f;

    int effectTime = 7;
    int enemyLayer; 

    bool IsEffect = false;
    public void StartCrisisAversion(int _player, int _enemyLayer)
    {
        //player = GameObject.Find(_player);
        player = Managers.game.RemoteTargetFinder(_player);
        effectOn = transform.GetChild(3).gameObject;

        pStats = player.GetComponent<PlayerStats>();

        enemyLayer = _enemyLayer;

    }

    public void Update()
    {
        // ���� �ǰ� 1�� �Ǿ��ٸ�....
        // �� ����ü�� �°� �ǰ� 1���Ϸ� �������� �ȴٸ� �ߵ�....
        // ���� �������� �̿ϼ� 
        if(pStats.nowHealth <= 0)
        {
            IsEffect = true;  
            effectOn.SetActive(IsEffect);
        }

        if(IsEffect == true)
        {
            //�� 1�� ����
            pStats.nowHealth = 1;

            nowEffectTime += Time.deltaTime;

            if (nowEffectTime >= effectTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
