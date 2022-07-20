using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Image))]
public class an_InventoryButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    InventoryItem correspondingInventoryItem;
    public void SetCorrespondingInventoryItem(InventoryItem _value)
    {
        correspondingInventoryItem = _value;
    }

    public InventoryItem GetCorrespondingInventoryItem() { return correspondingInventoryItem; }

    [Header("GRAPHICS")]
    [SerializeField] bool interactable = true;
    public bool IsInteractive => interactable;
    Color original_color = Color.white;
    [SerializeField] Color pressed_color;
    [SerializeField] Color chosen_color;
    public void SetButtonInteractive(bool _value) {
        interactable = _value;
    }
    AudioSource sound_to_play;
    Image image;
    TextMeshProUGUI text;
    public TextMeshProUGUI GetTextComponent()
    {
        return text;
    }
    public void SetText(string _value) { if (text != null) text.text = _value; }
    [Header("Values")]
    bool is_pressed;
    public bool isPressed => is_pressed;

    [System.Serializable]
    public class InventoryButtonEvent : UnityEvent<InventoryItem> { }

    public InventoryButtonEvent on_button_left_pressed = new InventoryButtonEvent();
    public InventoryButtonEvent on_button_left_released = new InventoryButtonEvent();

    public InventoryButtonEvent on_button_right_pressed = new InventoryButtonEvent();
    public InventoryButtonEvent on_button_right_released = new InventoryButtonEvent();

    private void Awake()
    {
        image = GetComponent<Image>();
        sound_to_play = GetComponent<AudioSource>();
        if (transform.childCount > 0)
            text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (interactable) original_color = image.color; else image.color = chosen_color;
        SetButtonInteractive(interactable);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            PointerDownLeft();
        else if (eventData.button == PointerEventData.InputButton.Right)
            PointerDownRight();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            PointerUpLeft();
        else if (eventData.button == PointerEventData.InputButton.Right)
            PointerUpRight();
    }

    protected virtual void PointerDownLeft()
    {
        if (!interactable) return;
        is_pressed = true;
        on_button_left_pressed?.Invoke(correspondingInventoryItem);
        image.color = pressed_color;
        if (sound_to_play != null)
            if (sound_to_play.clip != null)
                sound_to_play.Play();
    }

    protected virtual void PointerUpLeft()
    {
        if (!interactable) return;
        is_pressed = false;
        on_button_left_released?.Invoke(correspondingInventoryItem);
        image.color = original_color;
        ObjectsDatabase.singleton.inventory.RefreshInventoryUI();
    }

    protected virtual void PointerDownRight()
    {
        //if (!interactable) return;
        is_pressed = true;
        on_button_right_pressed?.Invoke(correspondingInventoryItem);
        image.color = pressed_color;
        if (sound_to_play != null)
            if (sound_to_play.clip != null)
                sound_to_play.Play();
    }

    protected virtual void PointerUpRight()
    {
        is_pressed = false;
        on_button_right_released?.Invoke(correspondingInventoryItem);
        image.color = original_color;
        ObjectsDatabase.singleton.inventory.RefreshInventoryUI();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetHoveredItemVariables(correspondingInventoryItem.GetCorrespondingItem().sprite ,correspondingInventoryItem.GetCorrespondingItem().description, 1.0f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //SetHoveredItemVariables(null, "", 0.0f);
    }

    void SetHoveredItemVariables(Sprite _sprite, string _desc, float _alpha)
    {
        ObjectsDatabase.singleton.inventory.SetHoveredItem(_sprite, _desc, _alpha);
    }

    
}
