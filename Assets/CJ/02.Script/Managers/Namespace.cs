using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using Structs;

public class Namespace : MonoBehaviour
{
    AttackData attackData = new AttackData();
    public float time;
    
    public void Start()
    {
        Utils.SetTimeScale(1);
        time = Time.fixedDeltaTime;
        
        if(attackData.attackType == AttackTypes.None)
        {
            Debug.Log('e');
        }

        if(Globals.WorldSpaceUISortingOrder == 1)
        {
            Debug.Log('g');
        }

        Debug.Log(Globals.UI.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}



