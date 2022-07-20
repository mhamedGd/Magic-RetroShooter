using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkItem : MonoBehaviour, ItemHand
{

    InventoryItem inventoryItem;
    public InventoryItem GetInventoryItem()
    {
        return inventoryItem;
    }
    public void SetInventoryItem(InventoryItem _itemToSet)
    {
        inventoryItem = _itemToSet;
    }

    public GameObject ItemObject()
    {
        return this.gameObject;
    }
    float tempItemOffset;
    public void Act()
    {
        tempItemOffset = ObjectsDatabase.singleton.itemsContainer.GetComponent<WeaponsContainer>().GetOffset;
        ObjectsDatabase.singleton.itemsContainer.GetComponent<WeaponsContainer>().SetOffset(0.0f);
        ObjectsDatabase.singleton.itemsContainer.transform.localPosition = Vector3.zero;
        consume = true;
    }

    public void StopActing()
    {
        ObjectsDatabase.singleton.itemsContainer.GetComponent<WeaponsContainer>().SetOffset(tempItemOffset);
        consume = false;
        consumingTimer = 0.0f;
        ObjectsDatabase.singleton.hudManager.SetChargingBar(0.0f);
        ReturnToOriginalSettings();
    }

    [SerializeField] int healthToIncrease;
    [SerializeField] int manaToIncrease;

    bool consume;
    [SerializeField] float timeToConsume;
    float consumingTimer;

    [SerializeField] Transform visualObject;
    Vector3 originalPosition;
    [SerializeField] Vector3 consumingPosition1;
    Vector3 toMoveToPosition;

    [SerializeField] AudioSource gulpSound;

    private void Start()
    {
        originalPosition = visualObject.localPosition;
        toMoveToPosition = consumingPosition1;
    }

    private void Update()
    {
        if (consume)
        {
            consumingTimer += Time.deltaTime;
            visualObject.localPosition = Vector3.Lerp(visualObject.localPosition, toMoveToPosition, 0.075f);
            visualObject.localRotation = Quaternion.Euler(Vector3.right * -60f);
            ObjectsDatabase.singleton.hudManager.SetChargingBar(consumingTimer/timeToConsume);
        }

        if(consumingTimer >= timeToConsume)
        {
            consume = false;
            consumingTimer = 0.0f;

            ReturnToOriginalSettings();

            if(GetInventoryItem().stock <= 1)
                ObjectsDatabase.singleton.itemsContainer.GetComponent<WeaponsContainer>().SetOffset(tempItemOffset);

            ObjectsDatabase.singleton.hudManager.SetChargingBar(0.0f);
            gulpSound.pitch = Random.Range(0.8f, 1.2f);
            gulpSound.Play();
            ObjectsDatabase.singleton.gridInventory.ConsumeItem(GetInventoryItem(), 1);

            ObjectsDatabase.singleton.playerStatus.AddHealth(healthToIncrease);
            ObjectsDatabase.singleton.playerStatus.AddMana(manaToIncrease);
        }
    }

    void ReturnToOriginalSettings()
    {
        visualObject.localPosition = originalPosition;
        visualObject.localRotation = Quaternion.Euler(Vector3.right * 0f);
    }
}
