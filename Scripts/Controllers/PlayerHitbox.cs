using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어 히트박스
public class PlayerHitbox : MonoBehaviour
{
    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController monster = collision.gameObject.GetComponent<MonsterController>();
        MonsterData monsterData = monster.MonsterData;
        if(monsterData != null && monster.IsAlive)
        {
            monsterData.Hitted(playerController.PlayerData.Attack,monster);
        }
    }
}
