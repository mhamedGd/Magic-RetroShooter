using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_CrouchState : m_BaseState
{
    public m_CrouchState (Agent _agent) : base(_agent) { }

    public override StateID GetState()
    {
        return StateID.Crouch;
    }

    CapsuleCollider[] colliders;
    public override void Entry()
    {
        base.Entry();
        colliders = agent.GetComponents<CapsuleCollider>();
        colliders[0].center = new Vector3(0, -0.5f, 0);
        colliders[1].center = new Vector3(0, -0.5f, 0);
        colliders[0].height /= 2f;
        colliders[1].height /= 2f;
        ObjectsDatabase.singleton.mainCamera.GetComponent<CameraController>().SetYOffset(agent.playerSettings.cameraCrouchedPosition);
        ObjectsDatabase.singleton.weaponsContainer.SetIntensity(agent.playerSettings.crouchingWeaponSway);
        stepsTimer = steps;
    }
    float steps = 1.2f;
    float stepsTimer;
    public override void Update()
    {
        base.Update();

        stepsTimer += Time.deltaTime;
        if (stepsTimer >= steps)
        {
            agent.GetPlayerSfxHandler().PlayCrouchStep();
            stepsTimer = 0.0f;
        }

        if (!IsGrounded())
            agent.statesHandler.ChangeState(StateID.Fall);

        bool something_up = Physics.OverlapSphere(agent.transform.position - Vector3.up * 0.1f, 0.3f, agent.playerSettings.groundMasks).Length > 0;

        if (agent.is_crouching || something_up)
            return;

        agent.statesHandler.ChangeState(StateID.Idle);
    }

    public override void FixedUpdate()
    {
        Move(agent.playerSettings.speed, agent.playerSettings.maxCrouchSpeed, 1.0f);
    }

    public override void Exit()
    {
        colliders[0].center = new Vector3(0, 0f, 0);
        colliders[1].center = new Vector3(0, 0f, 0);
        colliders[0].height *= 2f;
        colliders[1].height *= 2f;
        ObjectsDatabase.singleton.mainCamera.GetComponent<CameraController>().SetYOffset(agent.playerSettings.cameraStandingPosition);
    }
}
