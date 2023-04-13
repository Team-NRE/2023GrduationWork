using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public PlayerController player;
    public BaseController UI;
    public RespawnManager respawn;
    public CameraController Camera_Manager;
    
    void Awake()
    {
        instance = this;

        player = GameObject.Find("PlayerManager").GetComponent<PlayerController>();
        UI = GameObject.Find("BaseCard").GetComponent<BaseController>();
        respawn = GameObject.Find("RespawnManager").GetComponent<RespawnManager>();
        Camera_Manager = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }

}
   