using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float y_offset;
    public void SetYOffset(float _value) { y_offset = _value; }
    private void Start()
    {
        SetYOffset(ObjectsDatabase.singleton.playerAgent.playerSettings.cameraStandingPosition);
    }

    void Update()
    {
        //if (ObjectsDatabase.singleton.inventory.IsOn()) return;
        Look(ObjectsDatabase.singleton.playerAgent.playerSettings.cameraSensitivity);
        YPositionLerping();
    }   

    float x_rotation;
    float y_rotation;
    protected void Look(float _sensitivity)
    {
        float x_mouse = ObjectsDatabase.singleton.inputsHandler.GetDeltaMouse.x * _sensitivity;
        float y_mouse = ObjectsDatabase.singleton.inputsHandler.GetDeltaMouse.y * _sensitivity;
        x_rotation -= y_mouse;
        y_rotation += x_mouse;

        x_rotation = Mathf.Clamp(x_rotation, -80f, 80f);

        ObjectsDatabase.singleton.playerAgent.Look(y_rotation);
        ObjectsDatabase.singleton.mainCamera.localRotation = Quaternion.Euler(x_rotation, 0, 0);
    }

    public void YPositionLerping()
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Lerp(newPosition.y, y_offset, 0.15f);        
        //newPosition.y = Mathf.Clamp(newPosition.y, ObjectsDatabase.singleton.playerAgent.agentSettings.cameraCrouchedPosition, ObjectsDatabase.singleton.playerAgent.agentSettings.cameraStandingPosition);
        transform.localPosition = newPosition;
    }
}
