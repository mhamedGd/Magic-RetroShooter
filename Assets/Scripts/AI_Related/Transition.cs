using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateMachine/Transition")]
public sealed class Transition : ScriptableObject
{
    public Decision decision;
    public ai_BaseState trueState;
    public ai_BaseState falseState;

    public void Execute(BaseStateMachine _stateMachine)
    {
        if(decision.Decide(_stateMachine) && !(trueState is ai_RemainInState))
        {
            _stateMachine.currentState = trueState;
        }else if(!(falseState is ai_RemainInState))
        {
            _stateMachine.currentState = falseState;
        }
    }
}
