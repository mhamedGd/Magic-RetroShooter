using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    public void Act();
    public void StopActing();
    public GameObject GetGameObject();
    public Vector3 CursorSize();
}
