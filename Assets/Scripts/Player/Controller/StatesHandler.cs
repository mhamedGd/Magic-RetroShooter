using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateID { Idle, Walk, Sprint, Crouch, Jump, Fall,
    AI_Idle, AI_MoveToTarget
}

public class StatesHandler
{
    public StatesHandler(Agent _agent) {
        agent = _agent;
        registeredStates = new m_BaseState[Enum.GetNames(typeof(StateID)).Length];        
    }

    Agent agent;
    m_BaseState[] registeredStates;
    StateID currentState;

    public void RegisterState(m_BaseState _stateToRegister)
    {
        uint index = (uint)_stateToRegister.GetState();
        registeredStates[index] = _stateToRegister;
    }

    public m_BaseState GetState(StateID _stateToGet)
    {
        int index = (int)_stateToGet;
        return registeredStates[index];
    }
    
    public void ChangeState(StateID _ToState)
    {
        GetState(currentState)?.Exit();
        currentState = _ToState;
        GetState(currentState).Entry();
    }

    public StateID GetCurrentState()
    {
        return currentState;
    }

    public void UpdateState()
    {
        GetState(currentState).Update();
    }

    public void FixedUpdateState()
    {
        GetState(currentState).FixedUpdate();
    }
}
