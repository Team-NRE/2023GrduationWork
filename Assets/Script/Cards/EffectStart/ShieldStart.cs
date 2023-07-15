using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class ShieldStart : MonoBehaviour
{
    float defenceTime = 0.01f;
    Transform _player = null;
    float _Defence = default;
    bool stop = false;

    void Update()
    {
        if (stop == false)
        {
            StartShield(_player, _Defence);
        }
    }

    public void StartShield(Transform _Player, float _defence)
    {
        _player = _Player;
        _Defence = _defence;

        defenceTime += Time.deltaTime;

        if (defenceTime >= 2.0f)
        {
            PlayerStats _pStats = _player.gameObject.GetComponent<PlayerStats>();
            _pStats.defensePower -= _Defence;
            stop = true;
        }
    }


}
