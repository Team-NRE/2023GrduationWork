using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int CardCost = 1;

    private void Awake() {
            
    }
    
    public void cardEffect()
    {
        Debug.Log(CardCost);
    }
}
