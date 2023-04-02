using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterData : StatData
{
    public MonsterData(int id)
    {
        InitialSetStat(id);
    }

    public void InitialSetStat(int id)
    {
        Dictionary<int, Data.MonsterStat> dict = Managers.Data.MonsterStatDict;

        Data.MonsterStat stat = dict[id];

        Maxhp = stat.maxHp;
        Hp = Maxhp;
        Attack = stat.attack;
        Defense = stat.defense;
        TotalExp = stat.exp;

    }

    public void Hitted(int damage, MonsterController monster)
    {
        Hp -= damage;
        monster.Animator.SetTrigger(monster.AnimStrings.hit);
        CharacterEvents.s_characterDamaged.Invoke(monster.gameObject, damage);


        switch (monster.gameObject.name)
        {
            case "10":
                Managers.Sound.Play("Sounds/Effect/Cyberleaf-ModernUISFX/AlertsAndNotifications/GenericNotification4", Define.Sound.Effect);
                break;
            case "11":
                Managers.Sound.Play("Sounds/Effect/Cyberleaf-ModernUISFX/AlertsAndNotifications/GenericNotification11", Define.Sound.Effect);
                break;

        }


        if (Hp <= 0)
        {
            Hp = 0;
            monster.IsAlive = false;
            monster.gameObject.SetActive(false);
        }
        Debug.Log("Hp : " + Hp);
    }
}
