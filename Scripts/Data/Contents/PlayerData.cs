using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerData : StatData
{
    private UnityAction<int, int> _healthChanged = null;
    public UnityAction<int, int> HealthChanged { get { return _healthChanged; } set { _healthChanged = value; } }


    public PlayerData InitialSetStat(int level)
    {
        Dictionary<int, Data.PlayerStat> dict = Managers.Data.PlayerStatDict;

        Data.PlayerStat stat = dict[level];

        Maxhp = stat.maxHp;
        Hp = Maxhp;
        Attack = stat.attack;
        Defense = stat.defense;
        TotalExp = stat.totalExp;

        return this;
    }

    public void Hitted(int damage, PlayerController player)
    {
        Hp -= damage;
        HealthChanged.Invoke(Hp, Maxhp);   // ? 는 null인지 체크하는것
         player.Animator.SetTrigger(player.AnimStrings.hit);

        CharacterEvents.s_characterDamaged.Invoke(player.gameObject, damage);

        Managers.Sound.Play("Sounds/Effect/Cyberleaf-ModernUISFX/AlertsAndNotifications/SciFiNotification2", Define.Sound.Effect);

        if (Hp <= 0)
        {
            Hp = 0;
            player.IsAlive = false;
            CharacterEvents.s_playerDie.Invoke();
            //player.gameObject.SetActive(false);
        }
        Debug.Log("Hp : " + Hp);
    }
}
