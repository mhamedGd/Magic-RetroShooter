using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridInventory : MonoBehaviour
{
    [SerializeField] GameObject inventoryUIObject;
    [SerializeField] GameObject hud_inventoryUIObject;
    [SerializeField] Transform inventoryContentUI;
    [SerializeField] RectTransform itemElementPrefab;

    [SerializeField] RectTransform[] cells;
    [SerializeField] RectTransform[] hud_cells;
    [SerializeField] List<InventoryItem> inventory = new List<InventoryItem>();

    [SerializeField] RectTransform cursorItem;
    [SerializeField] RectTransform chosenItemTransform;
    [SerializeField] RectTransform hud_chosenItemTransform;
    public InfoWindow informationWindow;

    int inventoryIndex = 0;
    public int GetInventoryIndex => inventoryIndex;
    public void SetInventoryIndex(int _value)
    {
        inventoryIndex = _value;

        foreach(var it in inventory)
        {
            it.GetItemHand().ItemObject().SetActive(false);
        }

        chosenItemTransform.transform.SetParent(cells[inventoryIndex].transform);
        chosenItemTransform.anchoredPosition = Vector2.zero;

        hud_chosenItemTransform.transform.SetParent(hud_cells[inventoryIndex].transform);
        hud_chosenItemTransform.anchoredPosition = Vector2.zero;

        if (cells[inventoryIndex].GetComponent<InventoryObject>().isEmpty)
        {
            ObjectsDatabase.singleton.itemsInvoker.SetItemInHand(null);
            return;
        }
        ObjectsDatabase.singleton.itemsInvoker.SetItemInHand(GetThisInventoryItem(cells[inventoryIndex]).GetItemHand());
        GetThisInventoryItem(cells[inventoryIndex]).GetItemHand().ItemObject().SetActive(true);        
    }

    const int MAX_STOCK = 99;
    public int GetMaxStock() { return MAX_STOCK; }

    public void SetCursorSettings(Item _item, bool _value)
    {
        cursorItem.GetComponent<Image>().sprite = _item.sprite;
        cursorItem.gameObject.SetActive(_value);
        Cursor.visible = !_value;
    }

    [SerializeField] Item exampleItem;
    [SerializeField] Item exampleItem2;
    [SerializeField, HideInInspector] public InventoryItem tempItem = null;

    Canvas canvas;

    bool pause;
    public bool IsPause => pause;
    public void SetPause(bool _value)
    {
        pause = _value;
    }

    public int GetCellID(RectTransform _r)
    {
        for(int i = 0; i < cells.Length; i++)
        {
            if (cells[i] == _r)
                return i;
        }
        return -1;
    }

    public InventoryItem GetThisInventoryItem(RectTransform _cellRect)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].index == GetCellID(_cellRect))
                return inventory[i];
        }
        return null;
    }

    public void AddItem(Item _itemType, int _stockAmount)
    {
        int _stockAmountTemp = _stockAmount;

    CHECKSTOCKAMOUNT:

        //UPDATING THE HUD INVENTORY BAR TO UPDATE THE STOCK NUMBER
        
        //Checking items that are stackable and with less than MAX_STOCK
        foreach (var it in inventory)
        {
            if (_itemType == it.GetCorrespondingItem())
            {
                if (it.GetCorrespondingItem().stackable)
                {
                    if (it.stock < MAX_STOCK)
                    {
                        if (_stockAmountTemp <= 0) return;
                        it.stock++;
                        _stockAmountTemp--;
                        UpdateHudInventory();
                        //STOCK VISUALS
                        it.itemUIElement.GetComponent<ItemUIElement>().t_stock.text = "x" + it.stock.ToString();
                        SetUIItemSettings(it.itemUIElement.GetComponent<ItemUIElement>(), _itemType.sprite, it.stock);                                                
                        if (it.stock == MAX_STOCK) goto ADDITEMNEW;
                        goto CHECKSTOCKAMOUNT;
                    }
                    else
                    {
                        if (_stockAmountTemp <= 0) return;
                    }
                }
            }
        }

    ADDITEMNEW:
        //Checking to see if a slot is empty
        for(int i = 0; i < cells.Length; i++)
        {
            if (cells[i].GetComponent<InventoryObject>().isEmpty)
            {
                
                //Creating the hand object and the inventory object
                GameObject itemInstance = Instantiate(_itemType.prefab, ObjectsDatabase.singleton.itemsContainer.position, ObjectsDatabase.singleton.itemsContainer.rotation, ObjectsDatabase.singleton.itemsContainer);
                InventoryItem itemToAdd = new InventoryItem(_itemType, itemInstance.GetComponent<ItemHand>());
                itemInstance.GetComponent<ItemHand>().SetInventoryItem(itemToAdd);
                
                //Creating the UI ELEMENT and setting slot to be not empty
                RectTransform itemUI = Instantiate(itemElementPrefab.gameObject, Vector3.zero, Quaternion.identity, canvas.transform).GetComponent<RectTransform>();
                cells[i].GetComponent<InventoryObject>().SetIsEmpty(false);
                if(inventory.Count > 0)
                {
                    itemInstance.SetActive(false);
                }


                //Setting the UI Visuals
                itemUI.transform.SetParent(cells[i]);
                itemUI.anchoredPosition = Vector2.zero;                
                SetUIItemSettings(itemUI.GetComponent<ItemUIElement>(), _itemType.sprite, itemToAdd.stock);

                //Setting Item Details
                itemToAdd.index = i;
                itemToAdd.itemUIElement = itemUI;
                itemUI.GetComponent<ItemUIElement>().correspondingInventoryItem = itemToAdd;


                itemUI.gameObject.SetActive(true);
                inventory.Add(itemToAdd);


                SetInventoryIndex(inventoryIndex);
                //UPDATING THE HUD INVENTORY FOR THE NEWLY ADDED ITEM
                UpdateHudInventory();

                if (itemToAdd.GetCorrespondingItem().stackable)
                {
                    _stockAmountTemp--;                  

                    goto CHECKSTOCKAMOUNT;
                }
                
                return;
            }
            if (i == cells.Length - 1)
            {
                Vector3 dropPosition;
                if (Physics.Raycast(ObjectsDatabase.singleton.mainCamera.position, ObjectsDatabase.singleton.mainCamera.forward, out RaycastHit _hit, 2f, ObjectsDatabase.singleton.bulletMask))
                {
                    dropPosition = _hit.point + _hit.normal.normalized * 0.1f;
                }
                else { dropPosition = (ObjectsDatabase.singleton.mainCamera.position + ObjectsDatabase.singleton.mainCamera.forward * 1.5f); }

                GameObject itemToDrop = Instantiate(_itemType.pickup, dropPosition, Quaternion.identity);

                itemToDrop.GetComponent<ItemPickup>().SetStock(_stockAmountTemp);
            }
        }        
    }

    public InventoryItem AddItemInSpecificIndex(Item _itemType, int _stockAmount, int _index)
    {
        GameObject itemInstance = Instantiate(_itemType.prefab, ObjectsDatabase.singleton.itemsContainer.position, ObjectsDatabase.singleton.itemsContainer.rotation, ObjectsDatabase.singleton.itemsContainer);
        InventoryItem itemToAdd = new InventoryItem(_itemType, itemInstance.GetComponent<ItemHand>());

        itemInstance.GetComponent<ItemHand>().SetInventoryItem(itemToAdd);

        RectTransform itemUI = Instantiate(itemElementPrefab.gameObject, Vector3.zero, Quaternion.identity, canvas.transform).GetComponent<RectTransform>();

        if (inventory.Count > 0)
        {
            itemInstance.SetActive(false);
        }

        //cells[_index].GetComponent<InventoryObject>().SetIsEmpty(false);
        itemUI.GetComponent<ItemUIElement>().correspondingInventoryItem = itemToAdd;
        //itemUI.anchoredPosition = cells[_index].anchoredPosition;
        itemUI.gameObject.SetActive(false);
        itemUI.transform.SetParent(canvas.transform);
        itemToAdd.itemUIElement = itemUI;
        itemToAdd.stock = _stockAmount;
        //itemToAdd.index = _index;

        inventory.Add(itemToAdd);

        SetInventoryIndex(inventoryIndex);
        return itemToAdd;
    }

    public void RemoveItem(InventoryItem _itemToRemove)
    {
        if(_itemToRemove.itemUIElement != null) Destroy(_itemToRemove.itemUIElement.gameObject);
        if(_itemToRemove.GetItemHand().ItemObject() != null) Destroy(_itemToRemove.GetItemHand().ItemObject());
        //cells[_itemToRemove.index].GetComponent<InventoryObject>().SetIsEmpty(true);
        inventory.Remove(_itemToRemove);
        SetInventoryIndex(inventoryIndex);
    }

    public void ConsumeItem(InventoryItem _itemToConsume, int _consumeAmount)
    {
        _itemToConsume.stock -= _consumeAmount;
        if (_itemToConsume.stock <= 0)
        {
            cells[_itemToConsume.index].GetComponent<InventoryObject>().SetIsEmpty(true);
            RemoveItem(_itemToConsume);
        }
        SetUIItemSettings(_itemToConsume.itemUIElement.GetComponent<ItemUIElement>(), _itemToConsume.GetCorrespondingItem().sprite, _itemToConsume.stock);
        UpdateHudInventory();
    }

    public void SetUIItemSettings(ItemUIElement _itemToChange, Sprite _sprite, int _stock) {
        _itemToChange.m_sprite.sprite = _sprite;
        if (_stock > 1) _itemToChange.t_stock.gameObject.SetActive(true);
        else _itemToChange.t_stock.gameObject.SetActive(false);
        _itemToChange.t_stock.text = "x" + _stock.ToString();
    }

    public void UpdateHudInventory()
    {
        for (int i = 0; i < hud_cells.Length; i++)
        {
            ItemUIElement hudItemElement = hud_cells[i].GetComponent<ItemUIElement>();
            if (GetThisInventoryItem(cells[i]) != null)
            {
                hudItemElement.m_sprite.sprite = GetThisInventoryItem(cells[i]).GetCorrespondingItem().sprite;
                hudItemElement.m_sprite.gameObject.SetActive(true);
                if (GetThisInventoryItem(cells[i]).stock > 1)
                {
                    hudItemElement.t_stock.text = "x" + GetThisInventoryItem(cells[i]).stock;
                    hudItemElement.t_stock.gameObject.SetActive(true);
                }
                else { hudItemElement.t_stock.gameObject.SetActive(false); }
            }
            else
            {
                hudItemElement.m_sprite.gameObject.SetActive(false);
                hudItemElement.t_stock.gameObject.SetActive(false);
            }
        }
    }    

    public void DropItem(InventoryItem _itemToRemove)
    {
        Vector3 dropPosition;
        if (Physics.Raycast(ObjectsDatabase.singleton.mainCamera.position, ObjectsDatabase.singleton.mainCamera.forward, out RaycastHit _hit, 2f, ObjectsDatabase.singleton.bulletMask))
        {
            dropPosition = _hit.point + _hit.normal.normalized * 0.1f;
        }
        else { dropPosition = (ObjectsDatabase.singleton.mainCamera.position + ObjectsDatabase.singleton.mainCamera.forward * 1.5f); }

        ObjectsDatabase.singleton.itemsInvoker.SetItemInHand(null);

        //Spawn the pickup item at the drop position
        GameObject itemToDrop = Instantiate(_itemToRemove.GetCorrespondingItem().pickup, dropPosition, Quaternion.identity);
         
        itemToDrop.GetComponent<ItemPickup>().SetStock(tempItem.stock);
        RemoveItem(_itemToRemove);
        UpdateHudInventory();
    }

    public void TriggerInventory()
    {
        inventoryUIObject.SetActive(!inventoryUIObject.activeSelf);
        hud_inventoryUIObject.SetActive(!inventoryUIObject.activeSelf);

        Time.timeScale = inventoryUIObject.activeSelf ? 0.0f : 1.0f;
        SetPause(inventoryUIObject.activeSelf);

        Cursor.visible = inventoryUIObject.activeSelf;
        Cursor.lockState = Cursor.visible? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void Start()
    {
        canvas = ObjectsDatabase.singleton.canvas;

        AddItem(exampleItem2, 10);
        AddItem(exampleItem, 1);

        ObjectsDatabase.singleton.inputsHandler.onInventory.AddListener(TriggerInventory);

        SetInventoryIndex(0);
        UpdateHudInventory();
    }

    private void OnEnable()
    {
        tempItem = null;
    }

    private void Update()
    {
        Vector2 mouseToCanvasPosition = new Vector2(ObjectsDatabase.singleton.inputsHandler.GetMousePosition.x - Screen.width / 2, ObjectsDatabase.singleton.inputsHandler.GetMousePosition.y - Screen.height / 2) / canvas.scaleFactor; ;
        cursorItem.anchoredPosition = mouseToCanvasPosition;
        informationWindow.GetComponent<RectTransform>().anchoredPosition = mouseToCanvasPosition + new Vector2(informationWindow.GetComponent<RectTransform>().sizeDelta.x/2, 0);

    }

    public void UpdateInfoWindow(RectTransform _rect)
    {
        informationWindow.Label.text = GetThisInventoryItem(_rect).GetCorrespondingItem().name;
        informationWindow.Descrition.text = GetThisInventoryItem(_rect).GetCorrespondingItem().description;

        //informationWindow.Descrition.GetComponent<ExpandForText>().Expand(informationWindow.Descrition.GetComponent<TextMeshProUGUI>(),
        //    informationWindow.Descrition.GetComponent<RectTransform>());

    }
}
