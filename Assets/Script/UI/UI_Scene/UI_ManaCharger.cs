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
        //������ ������ �������� �κ��� �ʿ���
    }

    void Update()
    {
        ChargingBar(coolTime);
    }

    //���ȿ��� ��Ÿ���� �޾Ƽ� �ٸ� ä��� �κ�
    public void ChargingBar(float coolTime)
	{

	}

	//���� Mana1,2,3�� ������� �κ�
	public void ManaGem()
	{

	}
}
