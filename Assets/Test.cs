using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string path;
    public bool play;

    private void Awake()
    {
        path = "";
        play = false;
    }

    private void Update()
    {
        if (play)
        {
            Managers.Sound.Play(path);
            play = false;
        }
    }
}
