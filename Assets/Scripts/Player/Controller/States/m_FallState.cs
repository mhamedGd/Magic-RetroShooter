using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_FallState : m_BaseState
{
    public m_FallState(Agent _agent) : base(_agent) { }

    public override StateID GetState()
    {
        return StateID.Fall;
    }

    public override void Entry()
    {
        base.Entry();
    }

    public override void Update()
    {
        base.Update();

        if (IsGrounded())
            agent.statesHandler.ChangeState(StateID.Idle);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
