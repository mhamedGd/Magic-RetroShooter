using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    Agent agent;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        agent = GetComponent<Agent>();
        agent.statesHandler = new StatesHandler(agent);
        agent.statesHandler.RegisterState(new m_IdleState(agent));
        agent.statesHandler.RegisterState(new m_WalkState(agent));
        agent.statesHandler.RegisterState(new m_SprintState(agent));
        agent.statesHandler.RegisterState(new m_CrouchState(agent));
        agent.statesHandler.RegisterState(new m_JumpState(agent));
        agent.statesHandler.RegisterState(new m_FallState(agent));
        agent.statesHandler.ChangeState(agent.initialState);
    }

    private void Update()
    {
        agent.SetInputsDirection(ObjectsDatabase.singleton.inputsHandler.GetDirection);
        agent.SetSprinting(ObjectsDatabase.singleton.inputsHandler.IsSprinting);
        agent.SetCrouching(ObjectsDatabase.singleton.inputsHandler.IsCrouching);
    }
}
