using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_SprintState : m_BaseState
{
    public m_SprintState (Agent _agent) : base(_agent) { }

    public override StateID GetState()
    {
        return StateID.Sprint;
    }

    public override void Entry()
    {
        base.Entry();
        ObjectsDatabase.singleton.weaponsContainer.SetIntensity(agent.playerSettings.sprintingWeaponSway);
        stepsTimer = steps;
    }
    float steps = .4f;
    float stepsTimer;
    public override void Update()
    {
        base.Update();

        stepsTimer += Time.deltaTime;
        if (stepsTimer >= steps)
        {
            agent.GetPlayerSfxHandler().PlaySprintStep();
            stepsTimer = 0.0f;
        }

        if (!IsGrounded())
            agent.statesHandler.ChangeState(StateID.Fall);

        if (agent.is_sprinting && agent.inputsDirection != Vector2.zero)
            return;

        agent.statesHandler.ChangeState(StateID.Idle);
    }

    public override void FixedUpdate()
    {
        Move(agent.playerSettings.speed, agent.playerSettings.maxSprintSpeed, 1.0f);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
