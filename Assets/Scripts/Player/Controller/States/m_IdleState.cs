using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_IdleState : m_BaseState
{
    public m_IdleState(Agent _agent) : base(_agent) { }

    public override StateID GetState()
    {
        return base.GetState();
    }

    public override void Entry()
    {
        base.Entry();
        ObjectsDatabase.singleton.weaponsContainer.SetIsMoving(false);
    }

    public override void Update()
    {
        base.Update();

        if (!IsGrounded())
            agent.statesHandler.ChangeState(StateID.Fall);

        if (agent.is_crouching)
            agent.statesHandler.ChangeState(StateID.Crouch);

        if (agent.inputsDirection == Vector2.zero)
            return;

        agent.statesHandler.ChangeState(StateID.Walk);
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
