using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class InvincibleShieldStart : MonoBehaviour
{
    PlayerStats _pStats;
    ObjStats _oStats;
    Transform Player = null;
    float Defence = default;
    float Invincibility_Time = default;
    float Shield_Time = default;
    float pSave_Health;

    float time = 0.01f;

    bool stop = false;


    public void Invincibility(Transform _Player, float _defence, float _Invincibility_Time, float _Shield_Time)
    {
        Player = _Player;
        Defence = _defence;
        Invincibility_Time = _Invincibility_Time;
        Shield_Time = _Shield_Time;
    }


    public void Start()
    {
        Invincibility(Player, Defence, Invincibility_Time, Shield_Time);
        _pStats = Player.gameObject.GetComponent<PlayerStats>();
    }


    // Update is called once per frame
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

        if (time >= Invincibility_Time)
        {
            _pStats.defensePower -= Defence;

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

        if (time >= Shield_Time)
        {
            _pStats.nowHealth = pSave_Health;
            stop = false;

            time = 0;
            Destroy(this.gameObject);
        }
    }
}
