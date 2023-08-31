using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel; // Enum을 string으로 받기.

namespace Define
{
    public enum GameMode
	{
		Single,
		Multi,
	}
    
    public enum PlayerType 
    {
        Police,
        Firefighter,
        Lightsabre,
        Monk,
    }

    public enum PlayerTeam
    {
        HumanP1 = 1,
        CyborgP1 = 2,
        HumanP2 = 3,
        CyborgP2 = 4,
    }


    public enum State
    {
        Idle,
        Moving,
        Attack,
        Card,
        Skill,
        Die,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Pick,
        Game,
    }

    public enum Sound
    {
        Effect,
        Bgm,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum Layer
    {
        Default = 0,
        UI = 5,
        Human = 6,
        Cyborg = 7,
        Neutral = 8,
        Road = 9,
        Block = 10,
        Minimap = 11,
    }

    public enum CameraMode
    {
        QuaterView,
        FloatCamera,
    }

    public enum WorldObject
    {
        Unknown,
        Player,
        Minion,
        Objects,
    }

    public enum MouseEvent
    {
        PointerUp,
        Press,
        PointerDown,
        LeftButton,
    }

    public enum KeyboardEvent
    {
        Undefine,
        KeyUp,
        Q,
        W,
        E,
        R,
        A,
        U,
        P,
        Escape,
        Tab,
        TabUp,
        Space,
        SpaceUp,
    }

    
    public enum Projectile
    {
        Undefine,
        Attack_Proj,
        Skill_Proj,
    }

    public enum ObjectAction
    {
        Idle,
        Attack,
        Death,
        Move,
    }

    public enum ObjectType
    {
        Undefine,
        MeleeMinion,
        RangeMinion,
        SuperMinion,
        Tower,
        Nexus,
        Neutral,
    }

    public enum ObjectLine
    {
        UpperLine,
        LowerLine,
    }

    public enum ObjectPosArea
    {
        Undefine,
        Road,
        Building,
        MidWay,
        CenterArea,
    }
}