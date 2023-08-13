using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class ShieldStart : MonoBehaviour
{
    PlayerStats _pStats;

    float defence = default;
    float shield_Time = 0.01f;

    bool start;

    public void StartShield(string _player, float _defence)
    {
        _pStats = GameObject.Find(_player).GetComponent<PlayerStats>();
        defence = _defence;

        start = true;
    }

    public void Update()
    {
        if (start == true)
        {
            shield_Time += Time.deltaTime;

            if (shield_Time >= 2.0f)
            {
                _pStats.defensePower -= defence;
                start = false;
            }
        }
    }
}
