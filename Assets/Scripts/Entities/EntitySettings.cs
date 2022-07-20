using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EntitySettings")]
public class EntitySettings : ScriptableObject
{
    public float speed { get { return _speed; } private set { _speed = value; } }
    [SerializeField] float _speed;

    public float maxWalkSpeed { get { return _maxWalkSpeed; } private set { _maxWalkSpeed = value; } }
    [SerializeField] float _maxWalkSpeed;

    public float maxSprintSpeed { get { return _maxSprintSpeed; } private set { _maxSprintSpeed = value; } }
    [SerializeField] float _maxSprintSpeed;

    public float maxCrouchSpeed { get { return _maxCrouchSpeed; } private set { _maxCrouchSpeed = value; } }
    [SerializeField] float _maxCrouchSpeed;

    public float angularSpeed { get { return _angularSpeed; } private set { _angularSpeed = value; } }
    [SerializeField] float _angularSpeed;

    public float stoppingDistance {  get { return _stoppingDistance; } private set { _stoppingDistance = value; } }
    [SerializeField] float _stoppingDistance;
}
