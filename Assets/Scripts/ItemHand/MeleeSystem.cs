using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSystem : MonoBehaviour, ItemHand
{ 
    
    public GameObject ItemObject()
    {
        return this.gameObject;
    }

    InventoryItem inventoryItem;
    public InventoryItem GetInventoryItem()
    {
        return inventoryItem;
    }
    public void SetInventoryItem(InventoryItem _itemToSet)
    {
        inventoryItem = _itemToSet;
    }

    bool act;
    int index;
    [SerializeField] string[] animations;

    public void Act()
    {
        
    }

    public void StopActing()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
        animator.Play(animations[index]);
        index++;
        if (index >= animations.Length) index = 0;
    }

    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
