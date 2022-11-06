using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMapping : MonoBehaviour
{
    [Header ("---Store")]
    public GameObject StoreImg;
    bool CheckStoreImg = true;




    void Update()
    {
        if(Input.GetButtonDown("Option"))
        {
            Debug.Log("옵션");
        }       

        if(Input.GetButtonDown("Q"))
        {
            Debug.Log("Q");
        }

        if(Input.GetButtonDown("W"))
        {
            Debug.Log("W");
        }

        if(Input.GetButtonDown("E"))
        {
            Debug.Log("E");
        }

        if(Input.GetButtonDown("R"))
        {
            Debug.Log("R");
        }

        if(Input.GetButtonDown("Info"))
        {
            Debug.Log("캐릭터 정보");
        }

        //상점 정보
        if(Input.GetButtonDown("Store"))
        {
            switch(CheckStoreImg)
            {
                case true:
                {
                    StoreImg.SetActive(true);
                    CheckStoreImg = !CheckStoreImg;
                    return;
                }

                case false:
                {
                    StoreImg.SetActive(false);
                    CheckStoreImg = !CheckStoreImg;
                    return;
                }
            }
        }

        if(Input.GetButtonDown("Home"))
        {
            Debug.Log("귀한");
        }

        if(Input.GetButtonDown("Attack"))
        {
            Debug.Log("공격");
        }
    }

}
