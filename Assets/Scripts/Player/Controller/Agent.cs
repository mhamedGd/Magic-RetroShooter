using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] bool is_player;
    public bool IsPlayer => is_player;

    public StatesHandler statesHandler { get; set; }
    public StateID initialState;

    public PlayerSettingsObj playerSettings;

    public Vector2 inputsDirection { get; private set; }    
    public void SetInputsDirection(Vector2 _value) { inputsDirection = _value; }

    public bool is_sprinting { get; private set; }
    public void SetSprinting(bool _value) { is_sprinting = _value; }

    public bool is_crouching { get; private set; }
    public void SetCrouching(bool _value) { is_crouching = _value; }

    Rigidbody rb;
    public Rigidbody GetRigidbody() { return rb; }
    public PlayerSfxHandler GetPlayerSfxHandler() { return GetComponent<PlayerSfxHandler>(); }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //statesHandler = new StatesHandler(this);        
    }

    private void Update()
    {
        //if (ObjectsDatabase.singleton.inventory.IsOn()) return;
        statesHandler.UpdateState();
    }

    private void FixedUpdate()
    {
        //if (ObjectsDatabase.singleton.inventory.IsOn()) return;
        statesHandler.FixedUpdateState();
    }

    public void Look(float y_rotation)
    {
        GetRigidbody().MoveRotation(Quaternion.Euler(0, y_rotation + 180, 0));
    }
}
