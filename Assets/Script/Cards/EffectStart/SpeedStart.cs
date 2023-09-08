using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class SpeedStart : MonoBehaviour
{
    PlayerStats _pStats;

    float speed = default;
    float speed_Time = 0.01f;

    bool start = false;

    public void StartSpeed(int _player, float _speed)
    {
        //_pStats = GameObject.Find(_player).GetComponent<PlayerStats>();
        _pStats = Managers.game.RemoteTargetFinder(_player).GetComponent<PlayerStats>();
        speed = _speed;

        start = true;
    }

    public void Update()
    {
        if (start == true)
        {
            speed_Time += Time.deltaTime;

            if (speed_Time >= 4.99f)
            {
                _pStats.speed -= speed;
                start = false;
            }
        }
    }
}
