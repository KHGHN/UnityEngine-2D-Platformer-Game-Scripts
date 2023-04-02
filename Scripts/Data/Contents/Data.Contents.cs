using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// JSON 데이터 처리를 위한 클래스들
namespace Data
{

    #region PlayerStat
    [Serializable]          // 리플렉션 
    public class PlayerStat
    {
        public int level;       // 반드시 public으로 해야 밑에 FromJson이 작동함 public이 아니라 private으로 하고 싶다면 [SerializeField]를 변수에 붙이자
        public int maxHp;
        public int attack;
        public int defense;
        public int totalExp;
    }

    [Serializable]
    public class PlayerStatData : ILoader<int, PlayerStat>
    {
        public List<PlayerStat> playerStats = new List<PlayerStat>();   // 여기 이름이랑 JSON파일이름 말고 안에서 이름이랑 같아야 됨

        public Dictionary<int, PlayerStat> MakeDict()
        {
            Dictionary<int, PlayerStat> dict = new Dictionary<int, PlayerStat>();
            foreach (PlayerStat stat in playerStats)
            {
                dict.Add(stat.level, stat);
            }
            return dict;
        }
    }
    #endregion

    #region MonsterStat
    [Serializable]          // 리플렉션 
    public class MonsterStat
    {
        public int id;       // 반드시 public으로 해야 밑에 FromJson이 작동함 public이 아니라 private으로 하고 싶다면 [SerializeField]를 변수에 붙이자
        public int maxHp;
        public int attack;
        public int defense;
        public int exp;
    }

    [Serializable]
    public class MonsterStatData : ILoader<int, MonsterStat>
    {
        public List<MonsterStat> monsterStats = new List<MonsterStat>();

        public Dictionary<int, MonsterStat> MakeDict()
        {
            Dictionary<int, MonsterStat> dict = new Dictionary<int, MonsterStat>();
            foreach (MonsterStat stat in monsterStats)
            {
                dict.Add(stat.id, stat);
            }
            return dict;
        }
    }
    #endregion

    #region Item
    [Serializable]          // 리플렉션 
    public class Item
    {
        // 반드시 public으로 해야 밑에 FromJson이 작동함 public이 아니라 private으로 하고 싶다면 [SerializeField]를 변수에 붙이자
        public int item_id;                 // 아이템 ID
        public string item_name;            // 아이템 이름
        public string item_description;     // 아이템 설명
        public int item_type;               // 아이템 타입
        public int max_count;               // 최대 개수
        public int effect;                  // 사용 시 효과를 구현하기 위한 필드
        public int effect_type;             // 사용 시 효과를 구현하기 위한 필드
    }

    [Serializable]
    public class ItemData : ILoader<int, Item>
    {
        public List<Item> items = new List<Item>();

        public Dictionary<int, Item> MakeDict()
        {
            Dictionary<int, Item> dict = new Dictionary<int, Item>();
            foreach (Item item in items)
            {
                //Debug.Log("type : "+ item.item_type);
                dict.Add(item.item_id, item);
            }
            return dict;
        }
    }
    #endregion
}
