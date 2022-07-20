using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItem : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (ObjectsDatabase.singleton.gridInventory.tempItem == null) return;

        ObjectsDatabase.singleton.gridInventory.DropItem(ObjectsDatabase.singleton.gridInventory.tempItem);
        ObjectsDatabase.singleton.gridInventory.SetCursorSettings(ObjectsDatabase.singleton.gridInventory.tempItem.GetCorrespondingItem(), false);
        ObjectsDatabase.singleton.gridInventory.tempItem = null;
    }

}
