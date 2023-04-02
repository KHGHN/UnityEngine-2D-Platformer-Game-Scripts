using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveState : BaseState<MonsterController>
{
    float _walkSpeed = 3.0f;

    float _attackRange = 2.0f;

    Vector3 _desPos;

   

    public MonsterMoveState(MonsterController stateOwner, StateMachine<MonsterController> stateMachine, StatData data)
    : base(stateOwner, stateMachine, data) { }

    public override void Enter()
    {
        _stateOwner.Animator.SetBool(_stateOwner.AnimStrings.isMoving, true);
        
    }


    public override void UpdateLogic()
    {
        if(_stateOwner.TargetPlayer != null)
        {
            if (!_stateOwner.TargetPlayer.IsAlive)
            {
                _stateOwner.HasTarget = false;
                _stateOwner.TargetPlayer = null;
            }
        }

        _stateOwner.ChangeMoveDirection();
    }

    public override void UpdatePhysics()
    {
        if(_stateOwner.canMove && !_stateOwner.DetectCliff.IsCliffDetected)
        {
            _stateOwner.MovePhysics(_walkSpeed);
        }
    }

    public override void CheckSwitchState()
    {
        if (_stateOwner.HasTarget && !_stateOwner.DetectCliff.IsCliffDetected)
        {
            // 타겟이 있으면 타겟방향으로 공격범위 내까지 들어간 다음 Attack State로 변경
            _stateOwner.TargetDirectionAndMoveInput();
            _desPos = _stateOwner.TargetPlayer.transform.position;
            float distance = (_desPos - _stateOwner.transform.position).magnitude;
            if (distance <= _attackRange)
            {
                _stateMachine.ChangeState(_stateOwner.Dicstate[Define.MonsterState.Attack]);
            }
            
        }
    }


    public override void Exit()
    {
        _stateOwner.Animator.SetBool(_stateOwner.AnimStrings.isMoving, false);
        _stateOwner.CancelInvoke();
        _stateOwner.MovePhysics(0);
    }
}
