using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreboard : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreboardCanvas;

    private void Start() 
    {
        scoreboardCanvas.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboardCanvas.SetActive(true);
        }

        if(Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboardCanvas.SetActive(false);
        }
    }
}
