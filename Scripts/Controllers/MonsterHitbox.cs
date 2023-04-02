using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitbox : MonoBehaviour
{
    MonsterController monsterController;

    private void Awake()
    {
        monsterController = GetComponentInParent<MonsterController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        PlayerData playerData = player.PlayerData;
        if(playerData != null && player.IsAlive)
        {
            playerData.Hitted(monsterController.MonsterData.Attack,player);
        }
    }
}
