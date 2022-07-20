using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_JumpState : m_BaseState
{
    public m_JumpState(Agent _agent) : base(_agent) { }

    public override StateID GetState()
    {
        return StateID.Jump;
    }

    public override void Entry()
    {
        base.Entry();
        agent.GetRigidbody().velocity = new Vector3(agent.GetRigidbody().velocity.x, agent.playerSettings.jumpForce, agent.GetRigidbody().velocity.z);
        agent.GetPlayerSfxHandler().PlayJumpAudio();
    }

    public override void Update()
    {
        base.Update();
        //Move(agent.agentSettings.speed, agent.agentSettings.maxWalkSpeed, 0.5f);
        if (!IsGrounded() && agent.GetRigidbody().velocity.y < 0)
            agent.statesHandler.ChangeState(StateID.Fall);

        if (IsGrounded() && agent.GetRigidbody().velocity.y < 0)
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
