using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMapping : MonoBehaviour
{
    void Update() 
    {
        if(Input.GetKeyDown("q"))
        {
            Debug.Log("q");
        }  

        if(Input.GetKeyDown("w"))
        {
            Debug.Log("w");
        }  

        if(Input.GetKeyDown("e"))
        {
            Debug.Log("e");
        }  

        if(Input.GetKeyDown("r"))
        {
            Debug.Log("r");
        }  

        if(Input.GetKeyDown("tab"))
        {
            Debug.Log("캐릭터 정보");
        }  

        if(Input.GetKeyDown("p"))
        {
            Debug.Log("상점");
        }

        if(Input.GetKeyDown("escape"))
        {
            Debug.Log("옵션");
        }

        if(Input.GetKeyDown("a"))
        {
            Debug.Log("공격");
        }
    }
    
    
}
