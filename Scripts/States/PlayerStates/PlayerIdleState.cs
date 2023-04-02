using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : BaseState<PlayerController>
{
    public PlayerIdleState(PlayerController stateOwner, StateMachine<PlayerController> stateMachine, StatData data)
    : base(stateOwner, stateMachine, data){ } 

    public override void Enter()
    {

    }
    public override void UpdateLogic()
    {

    }
    public override void UpdatePhysics()
    {

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
            else if (_stateOwner.IsRunning)
            {
                _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Run]);
            }
        }
        else if (!_stateOwner.IsMoving)
        {
            if (_stateOwner.IsJumping)
            {
                _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Jump]);
            }
            else if (_stateOwner.IsAttacking)
            {
                _stateMachine.ChangeState(_stateOwner.Dicstate[Define.PlayerState.Attack]);
            }
        }

    }

    public override void Exit()
    {
    }
}
