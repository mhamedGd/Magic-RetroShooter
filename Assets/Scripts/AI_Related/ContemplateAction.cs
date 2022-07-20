using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AIStateMachine/Actions/Contemplate Action")]
public class ContemplateAction : FSMAction
{
    

    public override void Execute(BaseStateMachine _stateMachine)
    {
        NavMeshAgent navMeshAgent = _stateMachine.GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = true;
    }
}
