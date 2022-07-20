using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteract
{
    [SerializeField] Vector2 cursorSize;
    [SerializeField] Item correspondingItem;
    [SerializeField] int stock = 1;
    public int GetStock() { return stock; }
    public void SetStock(int _value) { stock = _value; }
    public void Act()
    {
        ObjectsDatabase.singleton.gridInventory.AddItem(correspondingItem, GetStock());
        Destroy(gameObject);
    }


    public void StopActing()
    {
    }
    public Vector3 CursorSize()
    {
        return new Vector3(cursorSize.x, cursorSize.y, 1);
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
