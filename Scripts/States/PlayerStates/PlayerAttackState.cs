using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : BaseState<PlayerController>
{

    public PlayerAttackState(PlayerController stateOwner, StateMachine<PlayerController> stateMachine, StatData data)
    : base(stateOwner, stateMachine, data) { }
    public override void Enter()
    {
        _stateOwner.Animator.SetTrigger(_stateOwner.AnimStrings.attack);
        Managers.Sound.Play("Sounds/Effect/Cyberleaf-ModernUISFX/SlidesAndTransitions/SwooshSlide1b", Define.Sound.Effect);
    }
    public override void UpdateLogic()
    {
        

    }
    public override void UpdatePhysics()
    {

    }

    public override void CheckSwitchState()
    {
        if (_stateOwner.CanMove)
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
        _stateOwner.IsAttacking = false;
        _stateOwner.MovePhysics(0);
    }
}
