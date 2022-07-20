using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public int index;
    public int stock = 1;
    public RectTransform itemUIElement;
    Item correspondingItem;
    public Item GetCorrespondingItem() { return correspondingItem; }
    
    ItemHand itemHand;
    public ItemHand GetItemHand() { return itemHand; }
    public InventoryItem(Item _correspondingItem, ItemHand _itemHand)
    {
        correspondingItem = _correspondingItem;
        itemHand = _itemHand;
    }
}
