using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : BaseState<PlayerController>
{
    float _runSpeed = 8.0f;
    float soundDelay = 0.15f;
    float DelayTime;

    public PlayerRunState(PlayerController stateOwner, StateMachine<PlayerController> stateMachine, StatData data)
    : base(stateOwner, stateMachine, data) { }

    public override void Enter()
    {
        _stateOwner.Animator.SetBool(_stateOwner.AnimStrings.isMoving, true);
        _stateOwner.Animator.SetBool(_stateOwner.AnimStrings.isRunning, true);
        Managers.Sound.Play("Sounds/Effect/RPG_Essentials_Free/12_Player_Movement_SFX/08_Step_rock_02", Define.Sound.Effect);
    }
    public override void UpdateLogic()
    {
        _stateOwner.ChangeMoveDirection();
    }
    public override void UpdatePhysics()
    {
        if(_stateOwner.CanMove && !_stateOwner.TouchingDirection.IsAirOnWall)
        {
            
            _stateOwner.MovePhysics(_runSpeed);

            DelayTime += Time.deltaTime;
            if (DelayTime >= soundDelay && _stateOwner.TouchingDirection.IsGrounded)
            {
                Managers.Sound.Play("Sounds/Effect/RPG_Essentials_Free/12_Player_Movement_SFX/08_Step_rock_02", Define.Sound.Effect);
                DelayTime = 0;
            }
        }
        else
        {
            _stateOwner.MovePhysics(0);
        }
    }
    public override void CheckSwitchState()
    {
        _stateOwner.PlayLandingSoundAfterFalling();

        if (_stateOwner.IsMoving)
        {
            if (!_stateOwner.IsRunning)
            {
                _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Walk]);
            }
            else if (_stateOwner.IsJumping)
            {
                _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Jump]);
            }
            else if (_stateOwner.IsAttacking)
            {
                _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Attack]);
            }
        }
        else if (!_stateOwner.IsMoving)
        {
            _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Idle]);
        }
    }

    public override void Exit()
    {
        _stateOwner.Animator.SetBool(_stateOwner.AnimStrings.isMoving, false);
        _stateOwner.Animator.SetBool(_stateOwner.AnimStrings.isRunning, false);
        _stateOwner.MovePhysics(0);
    }
}