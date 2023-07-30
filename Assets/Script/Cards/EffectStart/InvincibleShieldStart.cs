using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class InvincibleShieldStart : MonoBehaviour
{
    PlayerStats _pStats;

    float defence = default;
    float invincibility_Time = default;
    float shield_Time = default;
    float pSave_Health;

    float time = 0.01f;

    bool stop = false;

    public void Invincibility(string player, float _defence, float _invincibility_Time, float _shield_Time)
    {
        _pStats = GameObject.Find(player).GetComponent<PlayerStats>();

        defence = _defence;
        invincibility_Time = _invincibility_Time;
        shield_Time = _shield_Time;
    }

    void Update()
    {
        if (stop == false)
        {
            StartInvincibility();
        }

        if (stop == true)
        {
            Invoke("StartShield", 0.02f);
        }
    }


    public void StartInvincibility()
    {
        time += Time.deltaTime;

        if (time >= invincibility_Time)
        {
            //플레이어 방어력 빠짐
            _pStats.defensePower -= defence;

            pSave_Health = _pStats.nowHealth;

            Debug.Log(pSave_Health);

            _pStats.nowHealth += (_pStats.maxHealth / 100) * 10;

            stop = true;
            time = 0;
        }
    }

    public void StartShield()
    {
        time += Time.deltaTime;

        if (time >= shield_Time)
        {
            _pStats.nowHealth = pSave_Health;
            stop = false;

            time = 0;
            Destroy(this.gameObject);
        }
    }
}
