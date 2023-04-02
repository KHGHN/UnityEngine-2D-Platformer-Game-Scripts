using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Attack,
        Hurt,
        Death
    }

    public enum MonsterState
    {
        Idle,
        Move,
        Attack,
        Chase,
        Hurt,
        Death
    }


    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum ItemType
    {
        Consumables,
        Equipment,
        IncreasableCount,
        Quest,
        ETC,
        Empty,
    }

    public enum EffectType
    {
        HP,
    }
}
