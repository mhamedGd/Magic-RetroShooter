using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask collisionMasks;
    [SerializeField] GameObject impact;
    [SerializeField] float speed;
    [SerializeField, Range(0, 1)] float diff;


    Vector3 currentPosition;
    Vector3 lastPosition;
    bool checkDestroyed = false;

    private void Start()
    {
        if (Physics.Linecast(ObjectsDatabase.singleton.mainCamera.position, transform.position, out RaycastHit hit, collisionMasks))
        {
            DestroyProjectile(hit.point, hit.normal);
        }

        DestroyProjectile(transform.position, transform.forward.normalized, 10f);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        currentPosition = transform.position;

        transform.position += transform.forward * speed * Time.deltaTime;
        float rotationFactorX = Random.Range(-1.0f, 1.0f) * 5f * diff;
        float rotationFactorY = Random.Range(-1.0f, 1.0f) * 5f * diff;
        transform.Rotate(rotationFactorX, rotationFactorY, 0);

        if (lastPosition != Vector3.zero)
        {
            if (Physics.Linecast(lastPosition, currentPosition, out RaycastHit hit, collisionMasks))
            {                
                DestroyProjectile(hit.point, hit.normal);
            }
        }

        lastPosition = currentPosition;
    }


    void DestroyProjectile(Vector3 position, Vector3 rotation, float timer = 0.0f)
    {
        if (timer == 0.0f)
        {
            Instantiate(impact, position, Quaternion.LookRotation(rotation));
            checkDestroyed = true;
        }
        Destroy(gameObject, timer);
    }

    private void OnDestroy()
    {
        if(!checkDestroyed)
            Instantiate(impact, transform.position, Quaternion.LookRotation(transform.forward.normalized));
    }

}
