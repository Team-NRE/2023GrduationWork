using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel; // Enum을 string으로 받기.

namespace Define
{
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
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum KeyboardEvent
    {
        NoInput, 
        LeftButton,
        RightButton,
        Q,
        W,
        E,
        R,
        A,
        U,
    }

    public enum CardType
    {
        Undefine,
        NonProjective,
        Projective,
    }

    public enum Projectile
    {
        Undefine,
        Target,
        NonTarget,
    }

}