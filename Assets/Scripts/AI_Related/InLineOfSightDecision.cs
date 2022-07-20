using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateMachine/Decisions/In Line Of Sight")]
public class InLineOfSightDecision : Decision
{
    public override bool Decide(BaseStateMachine _stateMachine)
    {
        var enemyInLineOfSight = _stateMachine.GetComponent<EnemySightSensor>();
        return enemyInLineOfSight.Ping();
    }
}
