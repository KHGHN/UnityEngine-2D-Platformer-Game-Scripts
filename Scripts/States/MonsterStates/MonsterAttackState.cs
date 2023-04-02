using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : BaseState<MonsterController>
{
    float _attackRange = 2.0f;

    Vector3 _desPos;

    public MonsterAttackState(MonsterController stateOwner, StateMachine<MonsterController> stateMachine, StatData data)
    : base(stateOwner, stateMachine, data) { }

    public override void Enter()
    {
        _stateOwner.Animator.SetBool(_stateOwner.AnimStrings.isAttacking, true);
        _stateOwner.MovePhysics(0);
    }


    public override void UpdateLogic()
    {


        // ��Ÿ����� ������ �ٽ� move�� �Ѿư�
        if (_stateOwner.HasTarget)
        {
            _stateOwner.TargetDirectionAndMoveInput();

            _desPos = _stateOwner.TargetPlayer.transform.position;
            float distance = (_desPos - _stateOwner.transform.position).magnitude;
            if (distance > _attackRange)
            {
                _stateMachine.ChangeState(_stateOwner.Dicstate[Define.MonsterState.Move]);
            }

        }

        // �÷��̾� ��� �� ������� �ν��ϱ� ����
        if (_stateOwner.TargetPlayer != null)
        {
            if (!_stateOwner.TargetPlayer.IsAlive)
            {
                _stateOwner.HasTarget = false;
                _stateOwner.TargetPlayer = null;
            }
        }

    }

    public override void UpdatePhysics()
    {

    }

    public override void CheckSwitchState()
    {
        //Ÿ���� ������ ��� Move�� ������Ʈ ��ȯ
        if (!_stateOwner.HasTarget && !_stateOwner.DetectCliff.IsCliffDetected)
        {
            _stateOwner.TargetDirectionAndMoveInput();
            _stateOwner.TargetPlayer = null;
            _stateOwner.Invoke("DecideMoveOrIdle", 3);
            _stateMachine.ChangeState(_stateOwner.Dicstate[Define.MonsterState.Move]);

        }
    }


    public override void Exit()
    {
        _stateOwner.Animator.SetBool(_stateOwner.AnimStrings.isAttacking, false);
        _stateOwner.MovePhysics(0);
    }
}
