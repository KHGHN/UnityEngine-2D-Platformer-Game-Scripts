using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private BaseState<T> _currentState;
    public BaseState<T> CurrentState { get { return _currentState; } set { _currentState = value; } }

    private bool _isChangeState = false;
    public bool IsChangeState { get { return _isChangeState;} set { _isChangeState = value; } }

    public void Initialize(BaseState<T> startState)
    {
        _currentState = startState;
        _currentState.Enter();
    }

    public void ChangeState(BaseState<T> newState)
    {
        if(_currentState == newState)
        {
            return;
        }

        IsChangeState = true;

        _currentState.Exit();

        _currentState = newState;

        IsChangeState = false;

        _currentState.Enter();
    }
}
