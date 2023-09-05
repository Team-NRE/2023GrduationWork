using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Define;

public class UI_Select : UI_Scene
{
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
		Managers.game.myCharacterType = PlayerType.Police;
		Managers.game.myCharacterTeam = PlayerTeam.Human;
		Debug.Log($"MemberName : {Managers.game.myCharacterType}");
	}

	public void SpotOnFirefighter(PointerEventData data)
	{
		Managers.game.myCharacterType = PlayerType.Firefighter;
		Managers.game.myCharacterTeam = PlayerTeam.Human;
		Debug.Log($"MemberName : {Managers.game.myCharacterType}");

	}

	public void SpotOnLightsabre(PointerEventData data)
	{
		Managers.game.myCharacterType = PlayerType.Lightsabre;
		Managers.game.myCharacterTeam = PlayerTeam.Cyborg;
		Debug.Log($"MemberName : {Managers.game.myCharacterType}");
	}

	public void SpotOnMonk(PointerEventData data)
	{
		Managers.game.myCharacterType = PlayerType.Monk;
		Managers.game.myCharacterTeam = PlayerTeam.Cyborg;
		Debug.Log($"MemberName : {Managers.game.myCharacterType}");
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
