using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.PlayerStat> PlayerStatDict { get; private set; } = new Dictionary<int, Data.PlayerStat>();
    public Dictionary<int, Data.MonsterStat> MonsterStatDict { get; private set; } = new Dictionary<int, Data.MonsterStat>();

    public Dictionary<int, Data.Item> ItemDict { get; private set; } = new Dictionary<int, Data.Item>();

    public void Init()
    {
        PlayerStatDict = LoadJson<Data.PlayerStatData, int, Data.PlayerStat>("StatData").MakeDict();
        MonsterStatDict = LoadJson<Data.MonsterStatData, int, Data.MonsterStat>("MonsterData").MakeDict();

        ItemDict = LoadJson<Data.ItemData, int, Data.Item>("ItemData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
