using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Player_Instance;
    public PlayerAnimation player_ani;
    public PlayerMove player_move;
    public PlayerAttack player_att;
    public PlayerKey player_key;
    public PlayerStats player_stats;
    
    private void Awake()
    {
        Player_Instance = this;
    }
}