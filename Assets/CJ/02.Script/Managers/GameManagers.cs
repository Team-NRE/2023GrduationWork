using EnumTypes;
using System;
using UnityEngine;

//글로벌하게 사용되어야 하는 공통 데이터 타입들
namespace EnumTypes
{
    public enum AttackTypes
	{
		None, Melee, Range
	}

	public enum CardRanks
	{
		Normal, Special, Rare
	}

	public enum CardHowToUses
	{
		Normal, TargetGround, TargetEntity
	}

	public enum CardAfterUses
	{
		Discard, Destruct, Spawn
	}

	public enum GameFlowState
	{
		InitGame, SelectStage, Setting, Wave, EventFlow, Ending
	}
}


//여러타입이 하나로 묶여야 할때 사용
namespace Structs
{
    [Serializable]
	public struct AttackData
	{
		public AttackTypes attackType;
		public int attackAnimationIndex;

		
	}

	[Serializable]
	public struct StatModifierData
	{
		public float value;
	}
}


//유틸리티 함수는 일반화된 작업들을 수행하는 함수들로, 모두 static함수로 구성되어 있고 클래스 또한 static class로 정의
public static class Utils
{
	public static void SetTimeScale(float timescale)
	{
		Time.timeScale = timescale;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}

	public static int GenerateID<T>()
	{
		return GenerateID(typeof(T));
	}
	public static int GenerateID(System.Type type)
	{
		return Animator.StringToHash(type.Name);
	}

	public static float DirectionToAngle(float x, float y)
	{
		float cos = x;
		float sin = y;
		return Mathf.Atan2(sin, cos) * Mathf.Rad2Deg;
	}
    
}


//글로벌 변수들은 변하지 않는 변수들을 의미하므로 모두 const(상수)와 readonly(읽기 전용)로 정의
public static class Globals
{
	public const int WorldSpaceUISortingOrder = 1;
	public const int CharacterStartSortingOrder = 10;

	   
	public const string Default = "Default";
	public const string UI = "UI";
	public const string Card = "Card";
	public const string Obstacle = "Obstacle";
}

public interface IGameManagers
{
}

public class GameManagers : MonoBehaviour, IGameManagers
{
	[SerializeField]
	private UIManager _uiManager;

	
}