using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Minion = 8,
        Ground = 9,
        Player = 10,
        Block = 11
    }

    public enum CameraMode
    {
        QuaterView, //������ ī�޶�
        FloatCamera,//������ ī�޶�
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
        Tab,
        Q,
        W,
        E,
        R,
        Space,
        A
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
