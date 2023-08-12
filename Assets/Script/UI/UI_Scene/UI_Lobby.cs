using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Lobby : UI_Scene
{
	public enum Buttons
	{
		EnterSingle,
		EnterMulti,
	}


	public override void Init()
	{
		// ��� UI ��ü ���ε�
		Bind<Button>(typeof(Buttons));

		// ĳ���� ��ư Ŭ���� ���� ���� ǥ��
		GetButton((int)Buttons.EnterSingle).gameObject.BindEvent(EnterSingle);
		GetButton((int)Buttons.EnterMulti).gameObject.BindEvent(EnterMulti);
	}

	// Select Button Ŭ���� �߻��� �̺�Ʈ
	public void EnterSingle(PointerEventData data)
	{
		Debug.Log("EnterSingle");
		// 1. ������ ĳ���͸� ���� ��(GameScene)���� �ѱ��.
		SceneManager.LoadScene("Select");
	}

	// Select Button Ŭ���� �߻��� �̺�Ʈ
	public void EnterMulti(PointerEventData data)
	{
		Debug.Log("EnterMulti");
		// 1. ������ ĳ���͸� ���� ��(GameScene)���� �ѱ��.
		SceneManager.LoadScene("Select");
	}
}
