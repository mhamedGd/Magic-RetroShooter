using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    GridInventory gridInventory;
    Canvas canvas;

    [SerializeField] bool empty = true;
    public bool isEmpty => empty;
    public void SetIsEmpty(bool _value) { empty = _value; }


    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //If there is an item in this slot and cursor is not holding item
            if (gridInventory.tempItem == null)
            {
                if (empty) return;

                gridInventory.tempItem = gridInventory.GetThisInventoryItem(GetComponent<RectTransform>());
                gridInventory.tempItem.itemUIElement = transform.GetComponentInChildren<ItemUIElement>().transform.GetComponent<RectTransform>();
                gridInventory.tempItem.itemUIElement.transform.SetParent(canvas.transform);
                gridInventory.tempItem.itemUIElement.gameObject.SetActive(false);
                empty = true;
                gridInventory.SetCursorSettings(gridInventory.tempItem.GetCorrespondingItem(), true);
                //gridInventory.tempItem.index = 100;

                //UPDATE UI VISUALS
                gridInventory.UpdateHudInventory();

                gridInventory.SetUIItemSettings(gridInventory.tempItem.itemUIElement.GetComponent<ItemUIElement>(), gridInventory.tempItem.GetCorrespondingItem().sprite, gridInventory.tempItem.stock);
                if (gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()) != null)
                {
                    gridInventory.SetUIItemSettings(gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).itemUIElement.GetComponent<ItemUIElement>(),
                        gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).GetCorrespondingItem().sprite, gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock);
                }
            }
            else
            {
                //If there is an item in this slot and cursor is holding item
                if (!empty)
                {
                    if(gridInventory.tempItem.GetCorrespondingItem() == gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).GetCorrespondingItem())
                    {
                        //Stock the items in the current slot if they are matching and stackable
                        if (gridInventory.tempItem.GetCorrespondingItem().stackable)
                        {
                            if (gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock >= gridInventory.GetMaxStock()) return;

                            //Check if cursor stock + slot stock == more than MAX_STOCK
                            int finalStock = gridInventory.tempItem.stock + gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock;
                            if(finalStock >= gridInventory.GetMaxStock())
                            {
                                gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock = gridInventory.GetMaxStock();
                                gridInventory.tempItem.stock = finalStock - gridInventory.GetMaxStock();
                            }
                            else
                            {
                                gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock = finalStock;
                                gridInventory.tempItem.stock = 0;
                            }

                            //UPDATE UI VISUALS
                            gridInventory.SetUIItemSettings(gridInventory.tempItem.itemUIElement.GetComponent<ItemUIElement>(), gridInventory.tempItem.GetCorrespondingItem().sprite, gridInventory.tempItem.stock);
                            if (gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()) != null)
                            {
                                gridInventory.SetUIItemSettings(gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).itemUIElement.GetComponent<ItemUIElement>(),
                                    gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).GetCorrespondingItem().sprite, gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock);
                            }

                            if (gridInventory.tempItem.stock <= 0)
                            {                               

                                gridInventory.SetCursorSettings(gridInventory.tempItem.GetCorrespondingItem(), false);
                                //gridInventory.tempItem.index = 100;                                                                

                                gridInventory.RemoveItem(gridInventory.tempItem);
                                gridInventory.tempItem = null;
                            }                            

                            goto METHODEND;
                        }
                    }

                    //Here goes the item not being stackable or is not the same type to exchange it with the cursor item
                    InventoryItem secondInvTemp = gridInventory.tempItem;
                    gridInventory.tempItem = gridInventory.GetThisInventoryItem(GetComponent<RectTransform>());

                    gridInventory.tempItem.itemUIElement = transform.GetComponentInChildren<ItemUIElement>().transform.GetComponent<RectTransform>();
                    gridInventory.tempItem.itemUIElement.transform.SetParent(canvas.transform);
                    gridInventory.tempItem.itemUIElement.gameObject.SetActive(false);

                    secondInvTemp.itemUIElement.SetParent(this.transform);
                    secondInvTemp.index = gridInventory.GetCellID(GetComponent<RectTransform>());
                    secondInvTemp.itemUIElement.anchoredPosition = Vector2.zero;
                    secondInvTemp.itemUIElement.gameObject.SetActive(true);

                    //UPDATE UI VISUALS
                    gridInventory.SetCursorSettings(gridInventory.tempItem.GetCorrespondingItem(), true);
                    gridInventory.UpdateHudInventory();
                }
                //If there isn't an item in this slot and cursor is holding item
                else
                {
                    gridInventory.tempItem.itemUIElement.transform.SetParent(this.transform);
                    gridInventory.tempItem.index = gridInventory.GetCellID(GetComponent<RectTransform>());
                    gridInventory.tempItem.itemUIElement.anchoredPosition = Vector2.zero;
                    gridInventory.tempItem.itemUIElement.gameObject.SetActive(true);
                    empty = false;
                    gridInventory.SetCursorSettings(gridInventory.tempItem.GetCorrespondingItem(), false);
                    gridInventory.tempItem = null;

                    //UPDATE UI VISUALS
                    gridInventory.UpdateHudInventory();
                }

            }
            //Half the current slot if there is a stackable item in this slot and is more than 1 and temp is null
        }else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(gridInventory.tempItem == null)
            {
                if (!gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).GetCorrespondingItem().stackable) return;
                if (gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock <= 1) return;

                //Assigining temp item a new InventoryItem object with stock half of current slot
                gridInventory.tempItem = gridInventory.AddItemInSpecificIndex(gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()
                    ).GetCorrespondingItem(), gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock/2, gridInventory.GetCellID(GetComponent<RectTransform>()));
                
                //Check if item is even or odd to see compensate for the missing 1 if it is odd
                if (gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock % 2 == 0)
                {
                    gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock /= 2;
                }
                else
                {
                    gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock = (gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock / 2) + 1;
                }

                //Update UI Elements data
                gridInventory.SetUIItemSettings(gridInventory.tempItem.itemUIElement.GetComponent<ItemUIElement>(), gridInventory.tempItem.GetCorrespondingItem().sprite, gridInventory.tempItem.stock);
                gridInventory.SetUIItemSettings(gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).itemUIElement.GetComponent<ItemUIElement>(),
                    gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).GetCorrespondingItem().sprite, gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).stock);
                
                gridInventory.UpdateHudInventory();

                //Update UI Cursor data
                gridInventory.SetCursorSettings(gridInventory.tempItem.GetCorrespondingItem(), true);
            }
        }

        METHODEND:
        gridInventory.SetInventoryIndex(gridInventory.GetInventoryIndex);
        gridInventory.UpdateHudInventory();
    }

    // Start is called before the first frame update
    void Start()
    {
        gridInventory = ObjectsDatabase.singleton.gridInventory;
        canvas = ObjectsDatabase.singleton.canvas;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()) == null) return;
        /*
        gridInventory.informationWindow.Label.text = gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).GetCorrespondingItem().name;
        gridInventory.informationWindow.Descrition.text = gridInventory.GetThisInventoryItem(GetComponent<RectTransform>()).GetCorrespondingItem().description;
        
        gridInventory.informationWindow.Descrition.GetComponent<ExpandForText>().Expand(gridInventory.informationWindow.Descrition.GetComponent<TextMeshProUGUI>(),
            gridInventory.informationWindow.Descrition.GetComponent<RectTransform>());
        */

        gridInventory.UpdateInfoWindow(GetComponent<RectTransform>());

        gridInventory.informationWindow.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gridInventory.informationWindow.gameObject.SetActive(false);
    }
}
