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
        // 만약 피가 1이 되었다면....
        // 적 투사체에 맞고 피가 1이하로 떨어지게 된다면 발동....
        // 아직 스텟적용 미완성 
        if(pStats.nowHealth <= 0)
        {
            IsEffect = true;  
            effectOn.SetActive(IsEffect);
        }

        if(IsEffect == true)
        {
            //피 1로 고정
            pStats.nowHealth = 1;

            nowEffectTime += Time.deltaTime;

            if (nowEffectTime >= effectTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
