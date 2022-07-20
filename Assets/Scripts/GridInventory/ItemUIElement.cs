using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIElement : MonoBehaviour
{
    [HideInInspector] public InventoryItem correspondingInventoryItem;

    public Image m_sprite;
    public TextMeshProUGUI t_stock;
}
