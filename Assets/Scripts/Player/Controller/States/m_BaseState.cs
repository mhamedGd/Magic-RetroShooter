using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_BaseState
{

    public virtual StateID GetState() { return StateID.Idle; }
    public virtual void Entry() { }
    public virtual void Update() { ObjectsDatabase.singleton.weaponsContainer.SetIsMoving(agent.inputsDirection != Vector2.zero); }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }

    protected Agent agent;

    public m_BaseState(Agent _agent)
    {
        agent = _agent;
        ObjectsDatabase.singleton.inputsHandler.on_jump.AddListener(Jump);
    }

    protected void Move(float _speed, float _maxSpeed, float _control)
    {
        if (agent.GetRigidbody().velocity.magnitude >= _maxSpeed) return;

        Vector3 movementDirection = (agent.transform.forward * agent.inputsDirection.y + agent.transform.right * agent.inputsDirection.x);
        WalkOnSteps(movementDirection.normalized);
        if (OnSlope(movementDirection, out Vector3 _slopeDirection))
        {
            agent.GetRigidbody().AddForce(Vector3.down * 20, ForceMode.Force);
            agent.GetRigidbody().velocity += _slopeDirection * _speed * _control;
        }
        else
        {
            Vector3 to_be_velocity = _slopeDirection * _speed * _control;
            if (to_be_velocity.y > 0) to_be_velocity.y = 0f;
            agent.GetRigidbody().velocity += to_be_velocity;
            //agent.GetRigidbody().velocity = new Vector3(agent.GetRigidbody().velocity.x, _slopeDirection.y * _speed * _control, agent.GetRigidbody().velocity.z);
        }
        WalkOnSteps(movementDirection.normalized);
    }

    public bool OnSlope(Vector3 _movementDirection, out Vector3 _slopeDirection)
    {
        if (Physics.BoxCast(agent.transform.position, Vector3.one * .2f, Vector3.down, out RaycastHit _hit, Quaternion.identity, 1.3f, agent.playerSettings.groundMasks))
        {
            float angle = Vector3.Angle(Vector3.up, _hit.normal);
            _slopeDirection = Vector3.ProjectOnPlane(_movementDirection, _hit.normal).normalized;
            return angle <= agent.playerSettings.maxSlopeAngle;
        }

        _slopeDirection = _movementDirection;
        return false;
    }

    protected void WalkOnSteps(Vector3 _moveDirection)
    {
        if (Physics.Raycast((agent.transform.position - Vector3.up) + Vector3.up * 0.01f, _moveDirection, out RaycastHit _lowerhit, agent.GetComponent<CapsuleCollider>().radius + .6f, agent.playerSettings.groundMasks))
        {
            if (Vector3.Dot(Vector3.up, _lowerhit.normal) != 0.0f) return;

            if (Physics.Raycast(_lowerhit.point - _lowerhit.normal * .15f + Vector3.up, Vector3.down, out RaycastHit _upperhit, 1f, agent.playerSettings.groundMasks))
            {
                if (Mathf.Abs(_upperhit.point.y - _lowerhit.point.y) <= agent.playerSettings.maxStepHeight) {
                    //agent.GetRigidbody().position += new Vector3(0f, agent.playerSettings.smoothStep * Time.fixedDeltaTime, 0f);
                    agent.GetRigidbody().velocity += Vector3.up * agent.playerSettings.smoothStep;
                }
            }
        }

        
    }

    protected bool IsGrounded()
    {        
        return Physics.OverlapSphere(agent.transform.position + Vector3.down, 0.15f, agent.playerSettings.groundMasks).Length > 0;
    }

    protected void Jump()
    {
        if (agent.IsPlayer)
            if (IsGrounded() && agent.statesHandler.GetCurrentState() != StateID.Crouch)
                agent.statesHandler.ChangeState(StateID.Jump);
    }


}
