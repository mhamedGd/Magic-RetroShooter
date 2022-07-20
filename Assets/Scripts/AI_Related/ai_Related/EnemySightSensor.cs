using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightSensor : MonoBehaviour
{

    [HideInInspector] public Transform player;
    [SerializeField] LayerMask obstacleMasks;
    void Start()
    {
        player = ObjectsDatabase.singleton.playerAgent.transform;
    }

    public bool Ping()
    {
        Vector3 playerDirection = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, playerDirection, out RaycastHit _hit, 10, obstacleMasks))
            if (_hit.transform.tag == "Player")
                if (Vector3.Angle(transform.forward, (playerDirection)) < 60)
                    return true;

        return false;
    }
}
