using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : BaseState<PlayerController>
{
    float _walkSpeed = 5.0f;
    float soundDelay = 0.3f;
    float DelayTime;

    public PlayerWalkState(PlayerController stateOwner, StateMachine<PlayerController> stateMachine, StatData data)
    : base(stateOwner, stateMachine, data) { }

    public override void Enter()
    {
        _stateOwner.Animator.SetBool(_stateOwner.AnimStrings.isMoving, true);
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
            _stateOwner.MovePhysics(_walkSpeed);

            DelayTime += Time.deltaTime;
            if(DelayTime >= soundDelay && _stateOwner.TouchingDirection.IsGrounded)
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
            if (_stateOwner.IsRunning)
            {
                _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Run]);
            }
            else if(_stateOwner.IsJumping)
            {
                _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Jump]);
            }
            else if(_stateOwner.IsAttacking)
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
        _stateOwner.MovePhysics(0);
    }
}
