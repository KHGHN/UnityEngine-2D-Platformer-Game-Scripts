using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���Ͱ� �÷��̾ �����ϴ� �ڵ�
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
