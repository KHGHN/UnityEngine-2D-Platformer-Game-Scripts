using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : BaseState<PlayerController>
{
    float jumpPower = 10.0f;
    float airSpeed = 0;


    public PlayerJumpState(PlayerController stateOwner, StateMachine<PlayerController> stateMachine, StatData data)
    : base(stateOwner, stateMachine, data) { }

    public override void Enter()
    {
        Managers.Sound.Play("Sounds/Effect/RPG_Essentials_Free/12_Player_Movement_SFX/30_Jump_03", Define.Sound.Effect);

        _stateOwner.JumpPhysics(jumpPower);
        _stateOwner.Animator.SetTrigger(_stateOwner.AnimStrings.jump);
        if(_stateOwner.IsRunning)
        {
            airSpeed = 6.0f;
        }
        else if(!_stateOwner.IsRunning)
        {
            airSpeed = 3.0f;
        }
    }
    public override void UpdateLogic()
    {
        _stateOwner.ChangeMoveDirection();
    }
    public override void UpdatePhysics()
    {
        _stateOwner.AnimSetFloatRigidVeloY(_stateOwner.AnimStrings.yVelocity);

        if(!_stateOwner.TouchingDirection.IsAirOnWall)
        {
            _stateOwner.MovePhysics(airSpeed);
        }
    }

    public override void CheckSwitchState()
    {
        AnimatorStateInfo animInfo = _stateOwner.Animator.GetCurrentAnimatorStateInfo(0);
        if (animInfo.IsName(_stateOwner.AnimStrings.Player_Falling) && _stateOwner.TouchingDirection.IsGrounded)
        {
            if (_stateOwner.IsMoving)
            {
                if (!_stateOwner.IsRunning)
                {
                    _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Walk]);
                }
                else if (_stateOwner.IsRunning)
                {
                    _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Run]);
                }
            }
            else if (!_stateOwner.IsMoving)
            {
                _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Idle]);
            }
        }
    }

    public override void Exit()
    {
        _stateOwner.IsJumping = false;
        Managers.Sound.Play("Sounds/Effect/RPG_Essentials_Free/12_Player_Movement_SFX/45_Landing_01", Define.Sound.Effect);
        _stateOwner.JumpPhysics(0);
        _stateOwner.MovePhysics(0);
    }
}
