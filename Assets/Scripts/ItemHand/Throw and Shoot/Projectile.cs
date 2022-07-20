using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask collisionMasks;
    [SerializeField] GameObject impact;
    [SerializeField] float speed;
    public void SetSpeed(float _value) { speed = _value; }
    [SerializeField, Range(0, 1)] float diff;
    [SerializeField, Range(0.0f, 1.0f)] float gravity;


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
        velocity = transform.forward * speed/10;
    }

    Vector3 velocity;
    void FixedUpdate()
    {
        currentPosition = transform.position;

        //Randomness to movement
        float rotationFactorX = Random.Range(-1.0f, 1.0f) * .05f * diff;
        float rotationFactorY = Random.Range(-1.0f, 1.0f) * .05f * diff;
        
        //Velocity and Randomness Application        
        velocity += Vector3.down * Time.deltaTime * gravity * .5f + (transform.right * rotationFactorX + transform.up * rotationFactorY) * diff;
        
        transform.position += velocity;

        transform.Rotate(rotationFactorX, rotationFactorY, 0);

        //Line casts between previous fram position and current frame position
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
