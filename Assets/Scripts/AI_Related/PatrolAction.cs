using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AIStateMachine/Actions/Patrol Action")]
public class PatrolAction : FSMAction
{
    public override void Execute(BaseStateMachine _stateMachine)
    {
        var navMeshAgent = _stateMachine.GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(_stateMachine.Original_Position);
        /*var patrolPoints = _stateMachine.GetComponent<PatrolPoints>();
        
        if (patrolPoints.HasReached(navMeshAgent))
            navMeshAgent.SetDestination(patrolPoints.GetNext().position);*/
    }
}
