using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsDatabase : MonoBehaviour
{
    public Agent playerAgent;
    public PlayerStatus playerStatus;
    public Canvas canvas;
    public HudManager hudManager;
    public GridInventory gridInventory;
    public Inventory inventory;
    public Transform mainCamera;
    public Transform itemsContainer;
    public ItemsInvoker itemsInvoker;
    public LayerMask bulletMask;
    public WeaponsContainer weaponsContainer;
    public InputsHandler inputsHandler;

    public static ObjectsDatabase singleton;
    private void Awake()
    {
        if (singleton == null) singleton = this;
    }
}
