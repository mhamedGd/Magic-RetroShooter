using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour, ItemHand
{
    bool shoot;
    [SerializeField] WeaponData weaponData;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] LineRenderer bulletTrace;
    [SerializeField] GameObject bulletImpact;
    [SerializeField] AudioSource gunSfx;
    [SerializeField] ParticleSystem magicFlash;
    float timer;
    bool timeEffects;
    float effectsTimer;

    Vector3 originalPosition;
    [SerializeField] Transform itemObject;
    Quaternion originalRotation;
    float mStateRecoilOffset;    
    float shootingPeriodOffset;

    [SerializeField] float animSpeed;
    float animValue;

    private void OnEnable()
    {
        shoot = false;
        animValue = 0;
    }

    public void Act()
    {
        shoot = true;
    }

    public void StopActing()
    {
        shoot = false;
    }

    public GameObject ItemObject()
    {
        return this.gameObject;
    }

    InventoryItem itemInInventory;

    public InventoryItem GetInventoryItem()
    {
        return itemInInventory;
    }

    public void SetInventoryItem(InventoryItem _itemToSet)
    {
        itemInInventory = _itemToSet;
    }

    private void Start()
    {
        timer = weaponData._timeBetweenShots;
        originalPosition = transform.localPosition;
        originalRotation = itemObject.localRotation;
    }

    private void Update()
    {
        MovementBasedRecoilCalculation();
        WeaponTypeShooting();
    }

    void MovementBasedRecoilCalculation()
    {
        StateID currentMovementState = ObjectsDatabase.singleton.playerAgent.statesHandler.GetCurrentState();
        mStateRecoilOffset = weaponData._stateBasedRecoil[(int)currentMovementState];
    }

    void WeaponTypeShooting()
    {
        switch (weaponData._weaponType)
        {
            case WeaponType.Pistol:
                ShootingHandler(false);
                break;
            case WeaponType.Auto:
                ShootingHandler(true);
                break;
        }
    }

    bool shootAgain = true;
    void ShootingHandler(bool _shootAgain)
    {
        if (shoot)
        {
            animValue = Mathf.Clamp(animValue + Time.deltaTime * animSpeed, 0.0f, 1.0f);
            if(shootAgain) timer += Time.deltaTime;
            if (timer >= weaponData._timeBetweenShots && animValue == 1)
            {
                if (shootAgain)
                {
                    timer = 0.0f;
                    //DO SHOOTY STUFF
                    gunSfx.pitch = Random.Range(.75f, 1.5f);
                    gunSfx.Play();
                    //transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition + (Vector3.forward + Vector3.down/2) * -weaponData._recoil, weaponData._smoothTimer);
                    transform.localPosition = WeaponRecoil(transform, originalPosition + (Vector3.forward) * -weaponData._recoil, weaponData._smoothTimer);
                    magicFlash.Play();
                    //itemObject.localRotation = Quaternion.Lerp(itemObject.localRotation, Quaternion.Euler(originalRotation.x - 30, 0, 0), weaponData._smoothTimer*4);
                    /*
                    bulletTrace.SetPosition(0, muzzleFlash.transform.position);

                    //Calculating Bullet Tracjectory
                    if (Physics.Raycast(ObjectsDatabase.singleton.mainCamera.position, ObjectsDatabase.singleton.mainCamera.forward + ((ObjectsDatabase.singleton.mainCamera.up * Random.Range(-1f, 1f) +
                        ObjectsDatabase.singleton.mainCamera.right * Random.Range(-1f, 1f)) * (shootingPeriodOffset + mStateRecoilOffset)), out RaycastHit _hit, 100, ObjectsDatabase.singleton.bulletMask))
                    {
                        bulletTrace.SetPosition(1, _hit.point);
                        bulletTrace.gameObject.SetActive(true);
                        Instantiate(bulletImpact, _hit.point, Quaternion.LookRotation(_hit.normal));
                    }
                    else {
                        bulletTrace.SetPosition(1, transform.forward * 100);
                        bulletTrace.gameObject.SetActive(true);
                    }
                    muzzleFlash.SetActive(true);
                    */

                    //Firing Loop
                    for (int i = 0; i < weaponData._numOfProjectilesToFire; i++)
                    {
                        Instantiate(weaponData._projectile, muzzleFlash.transform.position, muzzleFlash.transform.rotation);
                    }
                    //Increasing Recoil Over Shooting period
                    shootingPeriodOffset += weaponData._recoilFactor;
                    shootingPeriodOffset = Mathf.Clamp(shootingPeriodOffset, 0, weaponData._maxTracerRecoil);
                    timeEffects = true;
                    shootAgain = _shootAgain;                    
                }
            }
            else { transform.localPosition = WeaponRecoil(transform, originalPosition, weaponData._smoothTimer / 4);
                //itemObject.localRotation = Quaternion.Lerp(itemObject.localRotation, originalRotation, weaponData._smoothTimer);
            }           
        }

        if (timeEffects)
        {
            effectsTimer += Time.deltaTime;
        }
        if (effectsTimer >= weaponData._timeToDisableShotsEffects)
        {
            timeEffects = false;
            effectsTimer = 0.0f;
            //DISABLE SHOOT STUFF
            //muzzleFlash.SetActive(false);
            bulletTrace.gameObject.SetActive(false);
        }

        if (!shoot)
        {            
            timer = weaponData._timeBetweenShots;
            shootingPeriodOffset = 0.0f;
            transform.localPosition = WeaponRecoil(transform, originalPosition ,weaponData._smoothTimer/4);
            itemObject.localRotation = Quaternion.Lerp(itemObject.localRotation, originalRotation, weaponData._smoothTimer);
            shootAgain = true;
            animValue = Mathf.Clamp(animValue - Time.deltaTime * animSpeed, 0.0f, 1.0f);
        }

        GetComponent<Animator>().SetFloat("Value", animValue);
    }

    Vector3 WeaponRecoil(Transform _t, Vector3 _supposedPosition ,float _smoothTimer)
    {
       return Vector3.Lerp(_t.localPosition, _supposedPosition, _smoothTimer);
    }    
}
