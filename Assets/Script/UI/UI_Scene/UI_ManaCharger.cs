using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ManaCharger : UI_Scene
{
    float coolTime = 2.0f;  //TODO : Get from stat

    enum GameObjects
    {
        ChargingBar,
        Energy1,
        Energy2,
        Energy3,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        //본인의 스탯을 가져오는 부분이 필요함
    }

    void Update()
    {
        ChargingBar(coolTime);
    }

    //스탯에서 쿨타임을 받아서 바를 채우는 부분
    public void ChargingBar(float coolTime)
	{

	}

	//사용시 Mana1,2,3이 사라지는 부분
	public void ManaGem()
	{

	}
}
