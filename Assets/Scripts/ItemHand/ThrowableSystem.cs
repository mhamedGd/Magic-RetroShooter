using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableSystem : MonoBehaviour, ItemHand
{

    InventoryItem inventoryItem;

    public InventoryItem GetInventoryItem()
    {
        return inventoryItem;
    }

    public GameObject ItemObject()
    {
        return this.gameObject;
    }

    public void SetInventoryItem(InventoryItem _itemToSet)
    {
        inventoryItem = _itemToSet;
    }

    bool charge = false;
    float chargeTimer = 0.0f;

    [SerializeField, Range(0.0f, 1.0f)] float chargeSpeed;
    [SerializeField, Range (0.0f, 0.5f)] float chargeRange;
    [SerializeField] float throwingForce;

    [SerializeField] Transform holder;
    Vector3 originalPosition;

    [SerializeField] GameObject projectilePrefab;

        

    public void Act()
    {
        charge = true;
    }

    public void StopActing()
    {
        charge = false;
        GameObject projectile = Instantiate(projectilePrefab, holder.position, holder.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * chargeTimer * throwingForce);
        ObjectsDatabase.singleton.gridInventory.ConsumeItem(GetInventoryItem(), 1);
        chargeTimer = 0;
    }
    private void Start()
    {
        originalPosition = holder.localPosition;
    }

    private void Update()
    {
        if (charge)
        {
            chargeTimer = Mathf.Clamp01(chargeTimer += Time.deltaTime * chargeSpeed);
        }

        holder.localPosition = originalPosition - Vector3.forward * chargeTimer * chargeRange;
    }

}
