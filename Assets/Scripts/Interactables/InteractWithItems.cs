using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithItems : MonoBehaviour
{
    [SerializeField] Transform worldCursor;
    [SerializeField] LayerMask interactableLayers;

    // Start is called before the first frame update
    void Start()
    {
        ObjectsDatabase.singleton.inputsHandler.interactPressed.AddListener(Interact);
        ObjectsDatabase.singleton.inputsHandler.interactReleased.AddListener(StopInteracting);
    }
    IInteract curentHoveredInteractable;
    [SerializeField] GameObject gg;
    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(ObjectsDatabase.singleton.mainCamera.position, ObjectsDatabase.singleton.mainCamera.forward, out RaycastHit _hit, 2f, interactableLayers))
        {
            curentHoveredInteractable = _hit.collider.GetComponent<IInteract>();
            if (curentHoveredInteractable != null)
            {
                gg = curentHoveredInteractable.GetGameObject();
                worldCursor.position = _hit.collider.transform.position;
                worldCursor.forward = ObjectsDatabase.singleton.mainCamera.forward;
                worldCursor.localScale = curentHoveredInteractable.CursorSize();
            }
            else
            {
                DisableCursor();
            }
        }
        else
        {
            DisableCursor();
        }
        
    }

    void DisableCursor()
    {
        worldCursor.position = Vector3.one * 1000f;
        curentHoveredInteractable = null;
    }

    public void Interact()
    {
        if (curentHoveredInteractable != null)
            gg.GetComponent<IInteract>().Act();
    }

    public void StopInteracting()
    {
        if (curentHoveredInteractable != null)
            gg.GetComponent<IInteract>().StopActing();
    }
}
