using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateMachine/Decisions/Out Of Line Of Sight")]
public class OutOfLineOfSightDecision : Decision
{

    float timer = 5f;
    public override bool Decide(BaseStateMachine _stateMachine)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 5f;
            return true;
        }
        return false;
    }
}
