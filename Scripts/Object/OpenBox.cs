using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenBox : MonoBehaviour
{

    private float grabCheckRadius = 1.5f;

    Collider2D stayCol;

    [SerializeField] private GameObject[] boxSprites = new GameObject[2];

    private void Start()
    {
        boxSprites[0].SetActive(true);
        boxSprites[1].SetActive(false);
    }

    private void Update()
    {
        if(stayCol != null)
        {
            
            PlayerController player = stayCol.gameObject.GetComponent<PlayerController>();
            if (player.IsInteract)
            {
                boxSprites[0].SetActive(false);
                boxSprites[1].SetActive(true);
                Debug.Log("Player interact");
            }
        }
    }

    private void FixedUpdate()
    {
        stayCol = Physics2D.OverlapCircle((Vector2)transform.position, grabCheckRadius, LayerMask.GetMask("Player"));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position, grabCheckRadius);
    }
}
