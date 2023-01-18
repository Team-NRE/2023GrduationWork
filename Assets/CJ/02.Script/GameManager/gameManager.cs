using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public PlayerManager player;
    public UIManager UI; 

    void Awake()
    {
        instance = this;
    }
}
