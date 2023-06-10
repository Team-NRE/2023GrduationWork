using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stat;

public class InvincibleShieldStart : MonoBehaviour
{
    Transform Player = null;
    float Defence = default;
    float Invincibility_Time = default;
    float Shield_Time = default;
    float Save_Health;
    float time = 0.01f;

    int stop = 0;


    // Update is called once per frame
    void Update()
    {
        if (stop == 0)
        {
            StartInvincibility(Player, Defence, Invincibility_Time, Shield_Time);
            Debug.Log("무적");
        }

        if(stop == 1)
        {
            StartShield();
            Debug.Log("방어막");
        }
    }

    public void StartInvincibility(Transform _Player, float _defence, float _Invincibility_Time, float _Shield_Time)
    {
        Player = _Player;
        Defence = _defence;
        Invincibility_Time = _Invincibility_Time;
        Shield_Time = _Shield_Time;

        time += Time.deltaTime;

        if(time >= Invincibility_Time)
        {
            PlayerStats _pStats = Player.gameObject.GetComponent<PlayerStats>();
            _pStats.defensePower -= Defence;
            
            Save_Health = _pStats.nowHealth;
            Debug.Log(Save_Health);
            
            _pStats.nowHealth +=  (_pStats.maxHealth / 100) * 33;
            stop = 1;
            time = 0;
        }
    }

    public void StartShield()
    {
        time += Time.deltaTime;

        if(time >= Shield_Time)
        {
            PlayerStats _pStats = Player.gameObject.GetComponent<PlayerStats>();
            _pStats.nowHealth =  Save_Health;
            stop = 2;
            Destroy(this.gameObject);
            time = 0;
        }
    }



}
