using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 몬스터가 플레이어를 감지하는 코드
public class DetectPlayer : MonoBehaviour
{
    MonsterController monsterController;

    // Start is called before the first frame update
    void Start()
    {
        monsterController = GetComponentInParent<MonsterController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            monsterController.HasTarget = true;
            monsterController.GetTargetInfo(collision.transform.GetComponent<PlayerController>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            monsterController.HasTarget = false;
        }
    }
}
