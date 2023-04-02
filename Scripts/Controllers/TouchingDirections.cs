using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// ����, ��, �� ��Ҵ��� ����
public class TouchingDirections : MonoBehaviour
{
    [SerializeField]
    private float _groundDistance = 0.1f;
    [SerializeField]
    private float ceilingDistance = 0.05f;

    [SerializeField]
    private ContactFilter2D _contactFilter;

    private RaycastHit2D[] _groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    private Vector2 grabRightOffset;
    private Vector2 grabLeftOffset;
    private float grabCheckRadius = 0.6f;


    private CapsuleCollider2D _touchingCol;

    private bool _isGrounded = false;
    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; } }

    private bool _isRightOnWall;
    public bool IsRightOnWall { get { return _isRightOnWall; } set { _isRightOnWall = value; } }

    private bool _isLeftOnWall;
    public bool IsLeftOnWall { get { return _isLeftOnWall; } set { _isLeftOnWall = value; } }

    private bool _isAirOnWall;
    public bool IsAirOnWall { get { return _isAirOnWall; } set { _isAirOnWall = value; } }

    private bool _isOnCeiling;
    public bool IsOnCeiling { get { return _isOnCeiling; } set { _isOnCeiling = value; } }


    private void Awake()
    {
        _touchingCol = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        _contactFilter.useLayerMask = true;
        _contactFilter.layerMask = LayerMask.GetMask("Ground");
        grabRightOffset = new Vector2(0.2f, 0f);
        grabLeftOffset = new Vector2(-0.2f, 0f);
    }

    private void FixedUpdate()
    {
        // ���� ��Ҵ��� üũ
        IsGrounded = _touchingCol.Cast(Vector2.down, _contactFilter, _groundHits, _groundDistance) > 0;

        // ���� ��Ҵ��� üũ
        IsRightOnWall = Physics2D.OverlapCircle((Vector2)transform.position + grabRightOffset, grabCheckRadius, LayerMask.GetMask("Ground"));
        IsLeftOnWall = Physics2D.OverlapCircle((Vector2)transform.position + grabLeftOffset, grabCheckRadius, LayerMask.GetMask("Ground"));

        // ������ �������� ���� �ɷ��� �� Move����
        if ((IsRightOnWall || IsLeftOnWall) && !IsGrounded)
        {
            IsAirOnWall = true;
        }
        else
        {
            IsAirOnWall = false;
        }

        // �Ӹ��� ��Ҵ��� üũ
        IsOnCeiling = _touchingCol.Cast(Vector2.up, _contactFilter, ceilingHits, ceilingDistance) > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + grabRightOffset, grabCheckRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + grabLeftOffset, grabCheckRadius);

    }

}
