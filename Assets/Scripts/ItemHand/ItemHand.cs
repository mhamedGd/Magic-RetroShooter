using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ItemHand
{
    public void Act();
    public void StopActing();

    public GameObject ItemObject();
    public InventoryItem GetInventoryItem();
    public void SetInventoryItem(InventoryItem _itemToSet);
}
