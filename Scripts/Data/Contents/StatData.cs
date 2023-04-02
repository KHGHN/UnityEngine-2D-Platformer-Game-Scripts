using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatData
{

    [SerializeField] private int _level;
    public int Level { get { return _level; } set {  _level = value; } }

    [SerializeField] private int _hp;
    public int Hp { get { return _hp; } set { _hp = value; } }

    [SerializeField] private int _maxHp;
    public int Maxhp { get { return _maxHp; } set { _maxHp = value; } }


    [SerializeField] private int _attack;
    public int Attack { get { return _attack; } set { _attack = value; } }

    [SerializeField] private int _defense;
    public int Defense { get { return _defense; } set { _defense = value; } }

    [SerializeField] private int _currentExp;
    public int CurrentExp { get { return _currentExp; } set { _currentExp = value; } }

    [SerializeField] private int _totalExp;
    public int TotalExp { get { return _totalExp; } set { _totalExp = value; } }
}


