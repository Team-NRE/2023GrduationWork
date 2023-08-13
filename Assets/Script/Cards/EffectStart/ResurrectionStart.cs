using Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionStart : MonoBehaviour
{
    GameObject player;


    public void StartResurrection(string _player)
    {
        player = GameObject.Find(_player);
        

    }

    public void Update()
    {
        
    }
}
