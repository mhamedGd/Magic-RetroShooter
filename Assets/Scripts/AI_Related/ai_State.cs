using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AIStateMachine/State")]
public class ai_State : ai_BaseState
{
    public List<FSMAction> action = new List<FSMAction>();
    public List<Transition> transitions = new List<Transition>();

    public override void Execute(BaseStateMachine _stateMachine)
    {
        foreach (var a in action)
            a.Execute(_stateMachine);

        foreach (var t in transitions)
            t.Execute(_stateMachine);
    }
}
