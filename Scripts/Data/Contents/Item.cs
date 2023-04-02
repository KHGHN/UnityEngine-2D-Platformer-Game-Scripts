using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Item : MonoBehaviour
{
    private int _item_id;
    public int Item_id { get { return _item_id; } }

    private string _item_name;
    public string Item_name { get { return _item_name; }}

    private string _item_description;
    public string Item_description { get { return _item_description; } }

    private Define.ItemType _itemType;
    public Define.ItemType Item_type { get { return _itemType; } }

    private int _maxCount;
    public int MaxCount { get { return _maxCount; } }

    private int _itemCount;
    public int ItemCount { get { return _itemCount; } }

    private int _effect;
    public int Effect { get { return _effect; } }

    private int _effectType;
    public int EffectType { get { return _effectType; } }

    private Sprite _itemIcon;
    public Sprite ItemIcon { get { return _itemIcon; } }

    private void Start()
    {
        SetInitialItemInfo();
    }
    public void SetInitialItemInfo(int itemCount = 1)
    {
        // 복제된 오브젝트 뒤에 붙는 Clone 때문에 오류나는거 막기
        if (gameObject.name.Contains("Clone"))
        {
            int index = gameObject.name.IndexOf("(Clone)");
            if (index > 0)
                gameObject.name = gameObject.name.Substring(0, index);
        }

        // JSON으로 만든 데이터 불러오기
        int item_id = int.Parse(gameObject.name);
        Dictionary<int, Data.Item> dict = Managers.Data.ItemDict;
        Data.Item item = dict[item_id];

        // 불러온 데이터 입력
        _item_id = item.item_id;
        _item_name = item.item_name;
        _item_description = item.item_description;
        _itemType = (Define.ItemType)item.item_type;
        _maxCount = item.max_count;
        _itemCount = itemCount;
        _effect = item.effect;
        _effectType = item.effect_type;
        _itemIcon = Managers.Resource.Load<SpriteRenderer>("Prefabs/Items/" + Item_id).sprite;

    }
    public void HealHP(PlayerController player)
    {
        if (player.PlayerData.Hp == player.PlayerData.Maxhp)
        {
            CharacterEvents.s_characterHealed.Invoke(player.gameObject, 1, "최대 체력입니다");
            return;
        }
        else
        {
            int DecreasedHpValue = player.PlayerData.Maxhp - player.PlayerData.Hp;
            player.PlayerData.Hp += Effect;

            if (player.PlayerData.Hp > player.PlayerData.Maxhp)
            {
                player.PlayerData.Hp = player.PlayerData.Maxhp;
            }



            player.PlayerData.HealthChanged.Invoke(player.PlayerData.Hp, player.PlayerData.Maxhp);


            if (DecreasedHpValue < Effect)
            {
                CharacterEvents.s_characterHealed.Invoke(player.gameObject, DecreasedHpValue, "");
            }
            else
            {
                CharacterEvents.s_characterHealed.Invoke(player.gameObject, Effect, "");
            }

            Managers.Sound.Play("Sounds/Effect/RPG_Essentials_Free/8_Buffs_Heals_SFX/02_Heal_02", Define.Sound.Effect);


            gameObject.SetActive(false);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            
            switch (_itemType)
            {
                case Define.ItemType.Consumables:
                    if((Define.EffectType)_effectType == Define.EffectType.HP)
                    {
                        HealHP(player);
                    }
                    break;
            }
        }
    }

}   
