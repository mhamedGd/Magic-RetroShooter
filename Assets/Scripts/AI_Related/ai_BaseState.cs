using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ai_BaseState : ScriptableObject
{
    public virtual void Execute(BaseStateMachine _stateMachine) { }
}
