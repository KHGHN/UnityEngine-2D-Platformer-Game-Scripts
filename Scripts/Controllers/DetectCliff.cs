using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 몬스터가 절벽 판단하는 코드
public class DetectCliff : MonoBehaviour
{
    //[SerializeField]
    //private float _groundDistance = 0.05f;

    //[SerializeField]
    //private ContactFilter2D _contactFilter;

    //private RaycastHit2D[] _groundHits = new RaycastHit2D[5];

    //private BoxCollider2D _cliffCol;

    MonsterController monster;

    Vector2 _offset;

    Vector3 _size;

    private bool _isCliffDetected = false;
    public bool IsCliffDetected { get { return _isCliffDetected; } set { _isCliffDetected = value; } }

    private void Awake()
    {
        monster = GetComponentInParent<MonsterController>();

        _offset = new Vector2(1.0f, -1.0f);
        _size = new Vector3(0.5f, 0.2f, 0);
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    _contactFilter.useLayerMask = true;
    //    _contactFilter.layerMask = LayerMask.GetMask("Ground");
    //}

    private void FixedUpdate()
    {

        Detect();

        //IsCliffDetected = _cliffCol.Cast(Vector2.down, _contactFilter, _groundHits, _groundDistance) == 0;
        //Debug.Log("Detectcliff status : "+IsCliffDetected);
        //Debug.Log(_cliffCol.Cast(Vector2.down, _contactFilter, _groundHits, _groundDistance));
    }

    public void Detect()
    {
        _offset.x = monster.transform.localScale.x * 1.5f;
        Collider2D col = Physics2D.OverlapBox((Vector2)transform.position + _offset, (Vector2)_size, LayerMask.GetMask("Ground"));

        if (col != null)
        {
            //Debug.Log("col!");
            _isCliffDetected = false;
        }
        else if (col == null)
        {
            //Debug.Log("no col!");
            _isCliffDetected = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + _offset, _size);
    }


    //    private void OnTriggerEnter2D(Collider2D collision)
    //    {
    //        IsCliffDetected = false;
    //    }

    //    private void OnTriggerExit2D(Collider2D collision)
    //    {
    //        IsCliffDetected = true;
    //    }
}


