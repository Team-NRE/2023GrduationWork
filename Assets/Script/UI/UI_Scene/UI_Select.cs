using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Define;

public class UI_Select : UI_Scene
{
	public static PlayerType _name;
	public static PlayerTeam _team;
	public GameMode gameMode;
	ColorBlock disabledColorBlock, selectedColorBlock;

	public enum Selectors
	{
		Police,
		Firefighter,
		Lightsabre,
		Monk,
	}

	public enum Buttons
	{
		Select,
	}

	public override void Init()
	{
		Bind<GameObject>(typeof(Selectors));
		Bind<Toggle>(typeof(Selectors));
		Bind<Button>(typeof(Buttons));

		Get<GameObject>((int)Selectors.Police).gameObject.BindEvent(SpotOnPolice);
		Get<GameObject>((int)Selectors.Firefighter).gameObject.BindEvent(SpotOnFirefighter);
		Get<GameObject>((int)Selectors.Lightsabre).gameObject.BindEvent(SpotOnLightsabre);
		Get<GameObject>((int)Selectors.Monk).gameObject.BindEvent(SpotOnMonk);

		Get<Toggle>((int)Selectors.Police).onValueChanged.AddListener(SpotOnCharacter);
		Get<Toggle>((int)Selectors.Firefighter).onValueChanged.AddListener(SpotOnCharacter);
		Get<Toggle>((int)Selectors.Lightsabre).onValueChanged.AddListener(SpotOnCharacter);
		Get<Toggle>((int)Selectors.Monk).onValueChanged.AddListener(SpotOnCharacter);

		GetButton((int)Buttons.Select).gameObject.BindEvent(SelectButton);

		InitColorBlock();
	}

	public override void UpdateInit()
    {
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			
		}
    }

	private void InitColorBlock()
	{
		disabledColorBlock = Get<Toggle>((int)Selectors.Police).colors;
		selectedColorBlock = ColorBlock.defaultColorBlock;
	}

	// 캐릭터 선택시 스팟이 켜지는 부분
	public void SpotOnPolice(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.Police).name;
		// 클릭 할 때 마다 string 객체에 버튼 이름을 저장한다.
		_name = PlayerType.Police;
		_team = PlayerTeam.Human;
		Debug.Log($"MemberName : {_name}");
	}

	public void SpotOnFirefighter(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.Firefighter).name;
		_name = PlayerType.Firefighter;
		_team = PlayerTeam.Human;
		Debug.Log($"MemberName : {_name}");

	}

	public void SpotOnLightsabre(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.Lightsabre).name;
		_name = PlayerType.Lightsabre;
		_team = PlayerTeam.Cyborg;
		Debug.Log($"MemberName : {_name}");
	}

	public void SpotOnMonk(PointerEventData data)
	{
		string name = Get<GameObject>((int)Selectors.Monk).name;
		_name = PlayerType.Monk;
		_team = PlayerTeam.Cyborg;
		Debug.Log($"MemberName : {_name}");
	}

	private void SpotOnCharacter(bool value)
	{
		Get<Toggle>((int)Selectors.Police).colors = (
			Get<Toggle>((int)Selectors.Police).isOn ? selectedColorBlock : disabledColorBlock
			);
		Get<Toggle>((int)Selectors.Firefighter).colors = (
			Get<Toggle>((int)Selectors.Firefighter).isOn ? selectedColorBlock : disabledColorBlock
			);
		Get<Toggle>((int)Selectors.Lightsabre).colors = (
			Get<Toggle>((int)Selectors.Lightsabre).isOn ? selectedColorBlock : disabledColorBlock
			);
		Get<Toggle>((int)Selectors.Monk).colors = (
			Get<Toggle>((int)Selectors.Monk).isOn ? selectedColorBlock : disabledColorBlock
			);
	}

	public void SelectButton(PointerEventData data)
	{
		Debug.Log("Start Game");
		// 1. 선택한 캐릭터를 다음 씬(GameScene)으로 넘긴다.
		SceneManager.LoadScene("View Test Scene");
	}
}
