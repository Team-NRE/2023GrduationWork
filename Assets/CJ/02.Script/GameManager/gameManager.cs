using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public PlayerManager player;
    public UIManager UI; 
    public RespawnManager respawn;

    void Awake()
    {
        instance = this;
    }

}
