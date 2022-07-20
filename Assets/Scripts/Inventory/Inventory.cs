using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject itemsContainer;
    public bool IsOn() { return itemsContainer.activeSelf; }

    [SerializeField] Transform scrollViewContent; 
    [SerializeField] RectTransform itemElementPrefab;

    List<InventoryItem> inventoryItems = new List<InventoryItem>();
    [SerializeField] InventoryItem[] handSlots;
    [SerializeField] GameObject[] handSlotsUIObjects;
    InventoryItem tempInvItem;
    public InventoryItem GetTempInvItem() { return tempInvItem; }
    int slotIndex;
    public void SetSlotIndex(int _value) { slotIndex = _value; }

    [SerializeField] Item firstItem;
    [SerializeField] GameObject slotsWindow;
    [SerializeField] Image hoveredItemImage;
    [SerializeField] TextMeshProUGUI hoveredItemDesc;

    public List<InventoryItem> GetInventoryItems() { return inventoryItems; }
    public InventoryItem GetItem(Item _itemTypeToGet)
    {
        foreach (var item in inventoryItems)
            if (item.GetCorrespondingItem() == _itemTypeToGet)
                return item;

        return null;
    }

    public InventoryItem AddItem(Item _itemToAdd, int stockAmount)
    {
        int tempAmount = stockAmount;
        LOOPAGAIN:
        //Instantiating the item in hand
        GameObject itemInstance = Instantiate(_itemToAdd.prefab, ObjectsDatabase.singleton.itemsContainer.position, ObjectsDatabase.singleton.itemsContainer.rotation, ObjectsDatabase.singleton.itemsContainer);

        //setting the inventoryitem of the hand object to a new inventoryitem object
        InventoryItem inventoryItemToAdd = new InventoryItem(_itemToAdd, itemInstance.GetComponent<ItemHand>());
        itemInstance.GetComponent<ItemHand>().SetInventoryItem(inventoryItemToAdd);

        //if the inventory isn't empty set the object to false
        if (inventoryItems.Count != 0) itemInstance.SetActive(false);

        //if the inventory item is empty then set the equipped item to this object
        if (inventoryItems.Count == 0)
        {
            ObjectsDatabase.singleton.itemsInvoker.SetItemInHand(inventoryItemToAdd.GetItemHand());
            tempInvItem = inventoryItemToAdd;
            InventoryToPocket(0);
        }

        foreach (var it in inventoryItems)
        {
            if (it.GetCorrespondingItem() == inventoryItemToAdd.GetCorrespondingItem())
            {                
                if (tempAmount > 0)
                {
                    if (it.stock == it.GetCorrespondingItem().maxStock)
                    {
                        continue;
                    }
                    it.stock += 1;
                    tempAmount--;
                    Destroy(itemInstance);
                    goto LOOPAGAIN;
                    
                }
                else
                {
                    goto RETURN;
                }
            }
        }
        inventoryItemToAdd.stock = 1;
        tempAmount--;
        inventoryItems.Add(inventoryItemToAdd);
        if (tempAmount > 0)
            goto LOOPAGAIN;
    RETURN:
        return inventoryItemToAdd;
    }   

    public void RemoveItem(ItemHand _itemToRemove)
    {
        //Checking if we have an item equipped        
        if (_itemToRemove == null) return;

        //Removing the item and destroy it if the stock is ZERO
        _itemToRemove.GetInventoryItem().stock--;
        if (_itemToRemove.GetInventoryItem().stock <= 0)
        {
            inventoryItems.Remove(_itemToRemove.GetInventoryItem());
            Destroy(_itemToRemove.ItemObject());
        }

        Vector3 dropPosition;
        if(Physics.Raycast(ObjectsDatabase.singleton.mainCamera.position, ObjectsDatabase.singleton.mainCamera.forward, out RaycastHit _hit, 2f, ObjectsDatabase.singleton.bulletMask))
        {
            dropPosition = _hit.point + _hit.normal.normalized * 0.1f;
        } else { dropPosition = (ObjectsDatabase.singleton.mainCamera.position + ObjectsDatabase.singleton.mainCamera.forward * 1.5f); }

        ObjectsDatabase.singleton.itemsInvoker.SetItemInHand(null);

        //Spawn the pickup item at the drop position
        Instantiate(_itemToRemove.GetInventoryItem().GetCorrespondingItem().pickup, dropPosition, Quaternion.identity);

        

        //Setting the item at first of the list as the current item
        if (inventoryItems.Count > 0)
        {
            ObjectsDatabase.singleton.itemsInvoker.SetItemInHand(inventoryItems[0].GetItemHand());
            inventoryItems[0].GetItemHand().ItemObject().SetActive(true);
        }
    }

    public void DropItem(InventoryItem _correspondingInventoryItem)
    {
        RemoveItem(_correspondingInventoryItem.GetItemHand());
        RefreshInventoryUI();
    }

    public void RefreshInventoryUI()
    {
        foreach (var e in scrollViewContent.GetComponentsInChildren<RectTransform>())
            if (e != scrollViewContent.GetComponent<RectTransform>())
                Destroy(e.gameObject);

        for(int i = 0; i < inventoryItems.Count; i++)
        {
            //Setting The UI Visuals Corresponding to each inventory Item
            GameObject elem = Instantiate(itemElementPrefab.gameObject, Vector3.zero, Quaternion.identity, scrollViewContent);
            elem.GetComponent<RectTransform>().anchoredPosition = itemElementPrefab.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, - itemElementPrefab.GetComponent<RectTransform>().sizeDelta.y * i);
            scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollViewContent.GetComponent<RectTransform>().sizeDelta.x, itemElementPrefab.GetComponent<RectTransform>().sizeDelta.y * (i + 1));
            scrollViewContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            SetUIElementItem(elem, inventoryItems[i]);

            //Subscribing to the item equipping process
            elem.GetComponent<an_InventoryButton>().SetCorrespondingInventoryItem(inventoryItems[i]);
            elem.GetComponent<an_InventoryButton>().on_button_left_pressed.AddListener(InventoryToTemp);
            elem.GetComponent<an_InventoryButton>().on_button_right_released.AddListener(DropItem);

            //Checking if the item we are equipping is corresponding to the current UI visual object to disable interactivity
            foreach (var t in handSlots)
            {
                if (t.GetItemHand() == inventoryItems[i].GetItemHand())
                {
                    elem.GetComponent<an_InventoryButton>().SetButtonInteractive(false);
                    elem.GetComponent<ItemUIObject>().s_inHand.gameObject.SetActive(true);
                }
            }

            elem.SetActive(true);
        }
    }

    void SetUIElementItem(GameObject elem, InventoryItem _invItem)
    {
        elem.GetComponent<ItemUIObject>().t_name.text = _invItem.GetCorrespondingItem().name;
        elem.GetComponent<ItemUIObject>().t_stock.text = _invItem.stock.ToString();
        if (_invItem.stock == 1) elem.GetComponent<ItemUIObject>().t_stock.gameObject.SetActive(false);
        elem.GetComponent<ItemUIObject>().s_sprite.sprite = _invItem.GetCorrespondingItem().sprite;
    }

    public void InventoryToTemp(InventoryItem _item)
    {
        tempInvItem = _item;
        slotsWindow.SetActive(true);
    }

    public void InventoryToPocket(int _index)
    {/*
        ObjectsDatabase.singleton.itemsInvoker.GetItemInHand().ItemObject().SetActive(false);
        ObjectsDatabase.singleton.itemsInvoker.SetItemInHand(_item.GetItemHand());
        ObjectsDatabase.singleton.itemsInvoker.GetItemInHand().ItemObject().SetActive(true);*/
        /*for (int i = 0; i < handSlots.Length; i++)
        {
            if (handSlots[i] == tempInvItem)
            {
                handSlots[i] = null;
                handSlotsUIObjects[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
        }*/
        handSlots[_index] = tempInvItem;
        ChangeHandItem(_index);
        
        for (int i = 0; i < handSlots.Length; i++)
        {
            if (handSlots[i].GetItemHand() != null)
            {
                handSlotsUIObjects[i].transform.GetChild(0).GetComponent<Image>().sprite = handSlots[i].GetCorrespondingItem().sprite;
                handSlotsUIObjects[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                handSlots[i] = null;
                handSlotsUIObjects[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
        }
        /*
        ObjectsDatabase.singleton.itemsInvoker.GetItemInHand().ItemObject().SetActive(false);
        ObjectsDatabase.singleton.itemsInvoker.SetItemInHand(handSlots[0].GetItemHand());
        ObjectsDatabase.singleton.itemsInvoker.GetItemInHand().ItemObject().SetActive(true);
        slotsWindow.SetActive(false);
        */
    }

    public void ChangeHandItem(int _index)
    {
        ObjectsDatabase.singleton.itemsInvoker.GetItemInHand().ItemObject().SetActive(false);
        ObjectsDatabase.singleton.itemsInvoker.SetItemInHand(handSlots[_index].GetItemHand());
        ObjectsDatabase.singleton.itemsInvoker.GetItemInHand().ItemObject().SetActive(true);
        slotsWindow.SetActive(false);
    }

    public void TriggerInventory()
    {
        itemsContainer.SetActive(!itemsContainer.activeSelf);
        if (itemsContainer.activeSelf) { Cursor.visible = true; Cursor.lockState = CursorLockMode.None; }
        else { Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; }
        PauseGame(IsOn());
        RefreshInventoryUI();
    }

    public void PauseGame(bool _value)
    {
        Time.timeScale = _value ? 0 : 1;
    }

    public void SetHoveredItem(Sprite _sprite, string _desc, float _alpha)
    {
        hoveredItemImage.sprite = _sprite;
        hoveredItemImage.color = new Color(hoveredItemImage.color.r, hoveredItemImage.color.g, hoveredItemImage.color.b, _alpha);
        hoveredItemDesc.text = _desc;
        hoveredItemDesc.color = new Color(hoveredItemDesc.color.r, hoveredItemDesc.color.g, hoveredItemDesc.color.b, _alpha);
    }

    private void Start()
    {
        AddItem(firstItem, 1);
        ObjectsDatabase.singleton.inputsHandler.onInventory.AddListener(TriggerInventory);
    }

}
