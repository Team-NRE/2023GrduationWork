using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMapping : MonoBehaviour
{
    [Header ("---Store")]
    public GameObject StoreImg;
    bool CheckStoreImg = true;


    private void Awake() {
        StoreImg.SetActive(false);
    }


    void Update()
    {
        //esc
        if(Input.GetButtonDown("Option"))
        {
            Debug.Log("옵션");
        }       

        //Q
        if(Input.GetButtonDown("Q"))
        {
            this.GetComponent<PlayerCard>().UseCard(0);
        }

        //W
        if(Input.GetButtonDown("W"))
        {
            this.GetComponent<PlayerCard>().UseCard(1);
        }

        //E
        if(Input.GetButtonDown("E"))
        {
            this.GetComponent<PlayerCard>().UseCard(2);
        }

        //R
        if(Input.GetButtonDown("R"))
        {
            this.GetComponent<PlayerCard>().UseCard(3);
        }

        //TAB
        if(Input.GetButtonDown("Info"))
        {
            Debug.Log("캐릭터 정보");
        }

        //P  
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

        //B
        if(Input.GetButtonDown("Home"))
        {
            Debug.Log("귀한");
        }

        //A
        if(Input.GetButtonDown("Attack"))
        {
            Debug.Log("공격");
        }
    }

}
