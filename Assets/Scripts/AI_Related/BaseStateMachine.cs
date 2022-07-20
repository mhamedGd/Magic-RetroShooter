using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    [SerializeField] private ai_BaseState initialState;

    private Dictionary<Type, Component> _cachedComponents;

    public Vector3 Original_Position;

    private void Awake()
    {
        currentState = initialState;
        _cachedComponents = new Dictionary<Type, Component>();
    }

    private void Start()
    {
        Original_Position = transform.position;
    }

    public ai_BaseState currentState { get; set; }

    private void Update()
    {
        currentState.Execute(this);
    }

    public new T GetComponent<T>() where T : Component
    {
        if (_cachedComponents.ContainsKey(typeof(T)))
            return _cachedComponents[typeof(T)] as T;

        var component = base.GetComponent<T>();
        if(component != null)
        {
            _cachedComponents.Add(typeof(T), component);
        }
        return component;
    }
}
