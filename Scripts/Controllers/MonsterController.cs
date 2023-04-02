using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private MonsterData _monsterData;
    public MonsterData MonsterData { get { return _monsterData; } }

    private Vector2 _moveInput;
    public Vector2 MoveInput { get { return _moveInput; } set { _moveInput = value; } }

    private Rigidbody2D _rigid;

    private StateMachine<MonsterController> _stateMachine;
    private Dictionary<Define.MonsterState, BaseState<MonsterController>> _dicState;
    public Dictionary<Define.MonsterState, BaseState<MonsterController>> Dicstate { get { return _dicState; } }

    private Animator _animator;
    public Animator Animator { get { return _animator; } }

    private AnimStrings _animStrings;
    public AnimStrings AnimStrings { get { return _animStrings; } }

    private TouchingDirections _touchingDirection;
    public TouchingDirections TouchingDirection { get { return _touchingDirection; } }

    private DetectCliff _detectCliff;
    public DetectCliff DetectCliff { get { return _detectCliff; } }

    private Collider2D _collider;

    private PlayerController _targetPlayer;
    public PlayerController TargetPlayer { get { return _targetPlayer; } set { _targetPlayer = value; } }


    //bool
    private bool _isMoving = false;
    public bool IsMoving { get { return _isMoving; } set { _isMoving = value; } }

    private bool _isAttacking = false;
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }

    private bool _hasTarget = false;
    public bool HasTarget { get { return _hasTarget; } set { _hasTarget = value; } }

    public bool canMove { get { return _animator.GetBool(AnimStrings.canMove); } }

    private bool _isAlive = false;
    public bool IsAlive { get { return _animator.GetBool(AnimStrings.isAlive); } set { _isAlive = value; _animator.SetBool(AnimStrings.isAlive, _isAlive); } }

    private bool isCoroutine = false;

    private void Awake()
    {
        

        // 물리
        _rigid = GetComponent<Rigidbody2D>();
        _touchingDirection = GetComponent<TouchingDirections>();
        _detectCliff = GetComponentInChildren<DetectCliff>();
        _collider = GetComponent<Collider2D>();

        // 애니메이션
        _animator = GetComponent<Animator>();
        _animStrings = new AnimStrings();

        // 스테이트머신
        _stateMachine = new StateMachine<MonsterController>();
        _dicState = new Dictionary<Define.MonsterState, BaseState<MonsterController>>();

        // 스테이트 생성 
        BaseState<MonsterController> monsterIdle = new MonsterIdleState(this, _stateMachine, _monsterData);
        BaseState<MonsterController> monsterMove = new MonsterMoveState(this, _stateMachine, _monsterData);
        BaseState<MonsterController> monsterAttack = new MonsterAttackState(this, _stateMachine, _monsterData);

        // 스테이트 Dic에 저장
        _dicState.Add(Define.MonsterState.Idle, monsterIdle);
        _dicState.Add(Define.MonsterState.Move, monsterMove);
        _dicState.Add(Define.MonsterState.Attack, monsterAttack);


    }

    private void Start()
    {
        // 복제된 오브젝트 뒤에 붙는 Clone 때문에 오류나는거 막기
        if (gameObject.name.Contains("Clone"))
        {
            int index = gameObject.name.IndexOf("(Clone)");
            if (index > 0)
                gameObject.name = gameObject.name.Substring(0, index);
        }

        //Debug.Log("Monster name : " + gameObject.name);

        // 몬스터 데이터 ( 스테이터스 등 )
        _monsterData = new MonsterData(int.Parse(gameObject.name));

        _stateMachine.Initialize(_dicState[Define.MonsterState.Idle]);
        Invoke("DecideMoveOrIdle", 3);
        //MoveInput = new Vector2(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("current state : " + _stateMachine.CurrentState);
        //Debug.Log("monster : " + _monsterData.Maxhp);

        //Debug.Log("Monster MoveInput x : " + MoveInput.x);


        //if(HasTarget)
        //{
        //    Debug.Log("Detect Player!");
        //}

        if (!_stateMachine.IsChangeState)
        {
            _stateMachine.CurrentState.CheckSwitchState();
            _stateMachine.CurrentState.UpdateLogic();
        }
    }

    private void FixedUpdate()
    {
        GroundedCheck();
        CliffCheck();
        if (!_stateMachine.IsChangeState)
        {
            _stateMachine.CurrentState.UpdatePhysics();
        }
    }

    public void GetTargetInfo(PlayerController _targetInfo)
    {
        _targetPlayer = _targetInfo;
    }

    public void GroundedCheck()
    {
        if (_touchingDirection.IsGrounded)
        {
            _animator.SetBool(AnimStrings.isGrounded, true);
        }
        else if (!_touchingDirection.IsGrounded)
        {
            _animator.SetBool(AnimStrings.isGrounded, false);
        }

    }

    public void CliffCheck()
    {
        if (DetectCliff.IsCliffDetected)
        {
            //Debug.Log("detectCliff!!");
            if(isCoroutine == false)
            {
                StartCoroutine(FlipDirectionCoroutine());
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }


    public void MovePhysics(float _currentSpeed)
    {
        _rigid.velocity = new Vector2(MoveInput.x * _currentSpeed, _rigid.velocity.y);
        //Debug.Log("currentSpeed : " + _currentSpeed);
    }

    public void ChangeMoveDirection()
    {
        if (MoveInput.x > 0)
        {
            // Face the right

            transform.localScale = new Vector2(1, 1);

        }
        else if (MoveInput.x < 0)
        {
            // Face the left
            transform.localScale = new Vector2(-1, 1);
        }
    }

    public void TargetDirectionAndMoveInput()
    {
        if (TargetPlayer == null)
        {
            return;
        }
        Vector2 targetDirection = ((TargetPlayer.transform.position - transform.position).normalized);
        MoveInput = new Vector2(Mathf.Round(targetDirection.x), 1);
    }


    public void DecideMoveOrIdle()
    {
        int decideValue = 0;
        decideValue = Random.Range(0, 2);

 

        switch ((Define.MonsterState)decideValue)
        {
            case Define.MonsterState.Idle:
                _stateMachine.ChangeState(Dicstate[Define.MonsterState.Idle]);
                break;
            case Define.MonsterState.Move:
                MoveInput = new Vector2(Random.Range(-1, 2), MoveInput.y);
                if(MoveInput.x == 0)
                {
                    _stateMachine.ChangeState(Dicstate[Define.MonsterState.Idle]);
                    break;
                }
                else
                {
                    _stateMachine.ChangeState(Dicstate[Define.MonsterState.Move]);
                    break;
                }
                
        }

        Invoke("DecideMoveOrIdle", 3);

    }

    public IEnumerator FlipDirectionCoroutine()
    {
        isCoroutine = true;
        MoveInput = new Vector2(MoveInput.x * -1, MoveInput.y);
        yield return new WaitForSeconds(3.0f);
        isCoroutine = false;
    }

    public void PlayAttackSound()
    {
        switch (gameObject.name)
        {
            case "10":
                Managers.Sound.Play("Sounds/Effect/Cyberleaf-ModernUISFX/SlidesAndTransitions/SwooshSlide3", Define.Sound.Effect);
                break;
            case "11":
                Managers.Sound.Play("Sounds/Effect/Cyberleaf-ModernUISFX/SlidesAndTransitions/LittleSwoosh4", Define.Sound.Effect);
                break;

        }
    }

}
