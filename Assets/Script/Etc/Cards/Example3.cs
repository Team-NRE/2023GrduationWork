using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example3 : BaseController
{
    public int CardCost = 1;
    PlayerStats _pStats;
    CardStats _cStats;

    private void Awake() {
            
    }
    
    public void cardEffect()
    {
        Debug.Log(CardCost);
    }
}
