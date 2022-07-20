using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInvoker : MonoBehaviour
{
    ItemHand itemInHand;
    public void SetItemInHand(ItemHand _itemInHand)
    {
        itemInHand = _itemInHand;
        if(itemInHand != null) Debug.Log(itemInHand.GetInventoryItem().GetCorrespondingItem().name);
    }

    public ItemHand GetItemInHand() { return itemInHand; }

    bool acting;
    public bool isActing => acting;

    private void Start()
    {
        ObjectsDatabase.singleton.inputsHandler.onLeftMouseButtonDown.AddListener(Act);
        ObjectsDatabase.singleton.inputsHandler.onLeftMouseButtonUp.AddListener(StopAct);
    }

    public void Act()
    {
        if (itemInHand != null)
        {
            itemInHand.Act();
            if(itemInHand is GunSystem)
            {
                acting = true;
            }
        }
    }

    public void StopAct()
    {
        if (itemInHand != null)
        {
            itemInHand.StopActing();
            if (itemInHand is GunSystem)
            {
                acting = false;
            }
        }
    }
}
