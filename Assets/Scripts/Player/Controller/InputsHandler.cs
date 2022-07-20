using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

public class InputsHandler : MonoBehaviour
{
    Vector2 direction;
    public Vector2 GetDirection => direction;

    public void OnMovement(InputValue _value)
    {
        direction = Vector2.zero;
        if (ObjectsDatabase.singleton.gridInventory.IsPause) return;
        direction = _value.Get<Vector2>();
    }

    Vector2 delta_mouse;
    public Vector2 GetDeltaMouse => delta_mouse;
    public void OnDeltaMouse(InputValue _value)
    {
        delta_mouse = Vector2.zero;
        if (ObjectsDatabase.singleton.gridInventory.IsPause) return;
        delta_mouse = _value.Get<Vector2>();
    }

    public UnityEvent on_jump;
    public void OnJump(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.IsPause) return;
        on_jump?.Invoke();
    }

    bool is_sprinting;
    public bool IsSprinting => is_sprinting;
    public void OnSprint(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.IsPause) return;
        is_sprinting = _value.isPressed;
    }

    bool is_crouching;
    public bool IsCrouching => is_crouching;
    public void OnCrouch(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.IsPause) return;
        is_crouching = _value.isPressed;
    }

    public UnityEvent onLeftMouseButtonDown;
    public UnityEvent onLeftMouseButtonUp;
    public void OnFire0(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.IsPause) return;
        if (_value.isPressed) onLeftMouseButtonDown?.Invoke(); else onLeftMouseButtonUp?.Invoke();
    }

    bool interact;
    public bool Interact => interact;
    public UnityEvent interactPressed;
    public UnityEvent interactReleased;
    public void OnInteract(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.IsPause) return;
        interact = _value.isPressed;
        if (interact) interactPressed?.Invoke(); else interactReleased?.Invoke();
    }

    public UnityEvent onDrop;
    public void OnDrop(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.IsPause) return;
        onDrop?.Invoke();
    }

    public UnityEvent onInventory;
    public void OnInventory(InputValue _value)
    {
        onInventory?.Invoke();
    }

    Vector2 mousePos;
    public Vector2 GetMousePosition => mousePos;
    public void OnMousePosition(InputValue _value)
    {
        mousePos = _value.Get<Vector2>();
    }

    //Scroll Through Numbers
    #region
    public void On_1(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.GetInventoryIndex == 0) return;
        ObjectsDatabase.singleton.gridInventory.SetInventoryIndex(0);
    }
    public void On_2(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.GetInventoryIndex == 1) return;
        ObjectsDatabase.singleton.gridInventory.SetInventoryIndex(1);
    }
    public void On_3(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.GetInventoryIndex == 2) return;
        ObjectsDatabase.singleton.gridInventory.SetInventoryIndex(2);
    }
    public void On_4(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.GetInventoryIndex == 3) return;
        ObjectsDatabase.singleton.gridInventory.SetInventoryIndex(3);
    }
    public void On_5(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.GetInventoryIndex == 4) return;
        ObjectsDatabase.singleton.gridInventory.SetInventoryIndex(4);
    }
    public void On_6(InputValue _value)
    {
        if (ObjectsDatabase.singleton.gridInventory.GetInventoryIndex == 5) return;
        ObjectsDatabase.singleton.gridInventory.SetInventoryIndex(5);
    }
    #endregion
}
