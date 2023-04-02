using UnityEngine;

public abstract class BaseState<T>
{
    protected T _stateOwner;
    protected StateMachine<T> _stateMachine;
    protected StatData _data;


    public BaseState(T stateOwner, StateMachine<T> stateMachine, StatData data)
    {
        _stateOwner = stateOwner;
        _stateMachine = stateMachine;
        _data = data;
    }

    public abstract void Enter();
    public abstract void UpdateLogic();
    public abstract void UpdatePhysics();
    public abstract void CheckSwitchState();
    public abstract void Exit();
}
