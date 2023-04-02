using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : BaseState<MonsterController>
{

    public MonsterIdleState(MonsterController stateOwner, StateMachine<MonsterController> stateMachine, StatData data)
    : base(stateOwner, stateMachine, data) { }

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
        if (_stateOwner.HasTarget)
        {
            _stateMachine.ChangeState(_stateOwner.Dicstate[Define.MonsterState.Move]);
        }
    }


    public override void Exit()
    {
        _stateOwner.CancelInvoke();
    }
}
