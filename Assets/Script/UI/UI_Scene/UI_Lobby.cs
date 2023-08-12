using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Lobby : UI_Scene
{
	public static string _name;

	public enum Pages
	{
		ModePage,
		SelectPage,
	}

	public enum Selectors
	{
		Police,
		FireFighter,
		LightSabre,
		Monk,
	}

	public enum Buttons
	{
		EnterSingle,
		EnterMulti,
	}


	public override void Init()
	{
		// ��� UI ��ü ���ε�
		Bind<GameObject>(typeof(Selectors));
		Bind<Button>(typeof(Buttons));

		// ĳ���� ��ư Ŭ���� ���� ���� ǥ��
		GetButton((int)Buttons.EnterSingle).gameObject.BindEvent(EnterSingle);
		GetButton((int)Buttons.EnterMulti).gameObject.BindEvent(EnterMulti);

		Get<GameObject>((int)Selectors.Police).gameObject.BindEvent(SpotOnPolice);
		Get<GameObject>((int)Selectors.FireFighter).gameObject.BindEvent(SpotOnFireFighter);
		Get<GameObject>((int)Selectors.LightSabre).gameObject.BindEvent(SpotOnFireLightSabre);
		Get<GameObject>((int)Selectors.Monk).gameObject.BindEvent(SpotOnFireMonk);
	}

	// Select Button Ŭ���� �߻��� �̺�Ʈ
	public void EnterSingle(PointerEventData data)
	{
		Debug.Log("EnterSingle");
		// 1. ������ ĳ���͸� ���� ��(GameScene)���� �ѱ��.
		//SceneManager.LoadScene("View Test Scene");

		GameObject page = Get<GameObject>((int)Pages.SelectPage);
		page.SetActive(true);
	}

	// Select Button Ŭ���� �߻��� �̺�Ʈ
	public void EnterMulti(PointerEventData data)
	{
		Debug.Log("EnterMulti");
		// 1. ������ ĳ���͸� ���� ��(GameScene)���� �ѱ��.
		SceneManager.LoadScene("View Test Scene");
	}

	public void GetSpot()
	{

	}

	public void SetName()
	{

	}

	// ĳ���� ���ý� ������ ������ �κ�
	public void SpotOnPolice(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.Police).gameObject.name;
		// 1. Ŭ���� �� ���� ������ ����
		Debug.Log(name);
		// 2. Ŭ�� �� �� ���� string ��ü�� ��ư �̸��� �����Ѵ�.
		_name = name;
		Debug.Log($"MemberName : {_name}");
	}

	public void SpotOnFireFighter(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.FireFighter).gameObject.name;
		Debug.Log(name);
		_name = name;
		Debug.Log($"MemberName : {_name}");

	}

	public void SpotOnFireLightSabre(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.LightSabre).gameObject.name;
		Debug.Log(name);
		_name = name;
		Debug.Log($"MemberName : {_name}");
	}

	public void SpotOnFireMonk(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.Monk).gameObject.name;
		Debug.Log(name);
		_name = name;
		Debug.Log($"MemberName : {_name}");
	}
}
