using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AIStateMachine/Actions/Chase Action")]
public class ChaseAction : FSMAction
{
    public override void Execute(BaseStateMachine _stateMachine)
    {
        var navMeshAgent = _stateMachine.GetComponent<NavMeshAgent>();
        var enemySightSensor = _stateMachine.GetComponent<EnemySightSensor>();
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(enemySightSensor.player.position);
    }
}
