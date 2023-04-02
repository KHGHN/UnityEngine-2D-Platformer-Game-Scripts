using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Cinemachine.DocumentationSortingAttribute;


// 플레이어 컨트롤러
public class PlayerController : MonoBehaviour, IDataPersistance
{
    [SerializeField]
    private PlayerData _playerData;
    public PlayerData PlayerData { get { return _playerData; } set { _playerData = value; } }

    private Vector2 _moveInput;
    public Vector2 MoveInput { get { return _moveInput; } }

    private Rigidbody2D _rigid;

    private StateMachine<PlayerController> _stateMachine;
    private Dictionary<Define.PlayerState, BaseState<PlayerController>> _dicState;
    public Dictionary<Define.PlayerState, BaseState<PlayerController>> Dicstate { get { return _dicState; } }

    private Animator _animator;
    public Animator Animator { get { return _animator; } }

    private AnimStrings _animStrings;
    public AnimStrings AnimStrings { get { return _animStrings; } }

    private TouchingDirections _touchingDirection;
    public TouchingDirections TouchingDirection { get { return _touchingDirection; } }

    private MonsterController _targetMonster;
    public MonsterController TargetMonster { get { return _targetMonster; } set { _targetMonster = value; } }

    private bool _isMoving = false;
    public bool IsMoving { get { return _isMoving; } set { _isMoving = value; } }

    private bool _isRunning = false;
    public bool IsRunning { get { return _isRunning; } set { _isRunning = value; } }

    private bool _isJumping = false;
    public bool IsJumping { get { return _isJumping; } set { _isJumping = value; } }

    private bool _isAttacking = false;
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }

    private bool _isInteract = false;
    public bool IsInteract { get { return _isInteract; } set { _isInteract = value; } }

    public bool CanMove { get { return _animator.GetBool(AnimStrings.canMove); } }

    public bool LockJump { get { return _animator.GetBool(AnimStrings.lockJump); } }

    private bool _isAlive = false;
    public bool IsAlive { get { return _animator.GetBool(AnimStrings.isAlive); } set { _isAlive = value; _animator.SetBool(AnimStrings.isAlive, _isAlive); } }

    private void Awake()
    {

        // 플레이어 데이터 ( 스테이터스 등 )
        PlayerData = new PlayerData();

        // 물리
        _rigid = GetComponent<Rigidbody2D>();
        _touchingDirection = GetComponent<TouchingDirections>();

        // 애니메이션
        _animator = GetComponent<Animator>();
        _animStrings = new AnimStrings();

        // 스테이트 머신 
        _stateMachine = new StateMachine<PlayerController>();
        _dicState = new Dictionary<Define.PlayerState, BaseState<PlayerController>>();

        // 스테이트 생성 
        BaseState<PlayerController> playerIdle = new PlayerIdleState(this, _stateMachine, _playerData);
        BaseState<PlayerController> playerWalk = new PlayerWalkState(this, _stateMachine, _playerData);
        BaseState<PlayerController> playerRun = new PlayerRunState(this, _stateMachine, _playerData);
        BaseState<PlayerController> playerJump = new PlayerJumpState(this, _stateMachine, _playerData);
        BaseState<PlayerController> playerAttack = new PlayerAttackState(this, _stateMachine, _playerData);

        // 스테이트 Dic에 저장
        _dicState.Add(Define.PlayerState.Idle, playerIdle);
        _dicState.Add(Define.PlayerState.Walk, playerWalk);
        _dicState.Add(Define.PlayerState.Run, playerRun);
        _dicState.Add(Define.PlayerState.Jump, playerJump);
        _dicState.Add(Define.PlayerState.Attack, playerAttack);
    }

    private void Start()
    {
        // 스테이트 머신 초기화 및 시작상태 설정
        _stateMachine.Initialize(_dicState[Define.PlayerState.Idle]);
    }

    private void Update()
    {
        if (!_stateMachine.IsChangeState)
        {
            _stateMachine.CurrentState.CheckSwitchState();
            _stateMachine.CurrentState.UpdateLogic();
        }
    }

    private void FixedUpdate()
    {
        GroundedCheck();
        if (!_stateMachine.IsChangeState)
        {
            _stateMachine.CurrentState.UpdatePhysics();
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if(IsAlive)
        {
            if (context.started)
            {
                
                _moveInput = context.ReadValue<Vector2>();
                IsMoving = true;
            }
            else if (context.canceled)
            {
                _moveInput = Vector2.zero;
                IsMoving = false;
            }
        }


    }


    public void OnRun(InputAction.CallbackContext context)
    {
        if(IsAlive)
        {
            if (!_animator.GetBool(AnimStrings.isFalling))
            {
                if (context.started)
                {
                    IsRunning = true;
                }
                else if (context.canceled)
                {
                    IsRunning = false;
                }
            }
        }
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (_touchingDirection.IsGrounded && IsAlive && !LockJump)
        {
            if (context.started)
            {
                IsJumping = true;
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (IsAlive)
        {
            if (context.started)
            {
                IsAttacking = true;
            }
            else if (context.canceled)
            {
                IsAttacking = false;
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (IsAlive)
        {
            if (context.started)
            {
                IsInteract = true;
            }
            else if (context.canceled)
            {
                IsInteract = false;
            }
        }
    }

    // 왼쪽,오른쪽 방향 입력에 따른 캐릭터 이미지 방향 전환
    public void ChangeMoveDirection()
    {
        if (_moveInput.x > 0)
        {
            // Face the right
            transform.localScale = new Vector2(1, 1);
        }
        else if (_moveInput.x < 0)
        {
            // Face the left
            transform.localScale = new Vector2(-1, 1);
        }
    }

    // 움직임 물리 처리
    public void MovePhysics(float _currentSpeed)
    {
        _rigid.velocity = new Vector2(_moveInput.x * _currentSpeed, _rigid.velocity.y);
    }

    // 점프
    public void JumpPhysics(float _jumpPower)
    {
        _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpPower);
    }

    // 지면 체크
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

    // 애니메이터 파라메터 조절
    public void AnimSetFloatRigidVeloY(string _animString)
    {
        _animator.SetFloat(_animString, _rigid.velocity.y);

    }

    // 점프 외 그냥 떨어져서 지면에 닿았을 때 소리 재생
    public void PlayLandingSoundAfterFalling()
    {
        if(_stateMachine.CurrentState == Dicstate[Define.PlayerState.Run] && !TouchingDirection.IsGrounded)
        {
            if(IsRunning)
            {
                IsRunning = false;
            }
        }

        AnimatorStateInfo animInfo = Animator.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName(AnimStrings.Player_Falling) && TouchingDirection.IsGrounded)
        {
            Managers.Sound.Play("Sounds/Effect/RPG_Essentials_Free/12_Player_Movement_SFX/45_Landing_01", Define.Sound.Effect);
        }


    }

    public void LoadData(GameData data)
    {
        if (!data.IsNewGame)
        {
            _playerData.Level = 1;
            _playerData.InitialSetStat(_playerData.Level);
        }
        else
        {
            _playerData.Level = data.PlayerData.Level;
            _playerData.Hp = data.PlayerData.Hp;
            _playerData.Maxhp = data.PlayerData.Maxhp;
            _playerData.Attack = data.PlayerData.Attack;
            _playerData.Defense = data.PlayerData.Defense;
            _playerData.CurrentExp = data.PlayerData.CurrentExp;
            _playerData.TotalExp = data.PlayerData.TotalExp;
            this.transform.position = data.PlayerPosition;
        }

        
    }

    public void SaveData(ref GameData data)
    {
        data.PlayerPosition = this.transform.position;

        data.PlayerData.Level = _playerData.Level;
        data.PlayerData.Hp = _playerData.Hp;
        data.PlayerData.Maxhp = _playerData.Maxhp;
        data.PlayerData.Attack = _playerData.Attack;
        data.PlayerData.Defense = _playerData.Defense;
        data.PlayerData.CurrentExp = _playerData.CurrentExp;
        data.PlayerData.TotalExp = _playerData.TotalExp;

    }
}
