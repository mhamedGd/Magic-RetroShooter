using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Pistol, Auto }

[CreateAssetMenu(menuName ="WeaponData")]
public class WeaponData : ScriptableObject
{
    
    [SerializeField] WeaponType weaponType;
    public WeaponType _weaponType => weaponType;

    [SerializeField] GameObject projectile;
    public GameObject _projectile => projectile;

    [SerializeField] float numOfProjectilesToFire = 1;
    public float _numOfProjectilesToFire => numOfProjectilesToFire;

    [SerializeField] float timeBetweenShots;
    public float _timeBetweenShots => timeBetweenShots;

    [SerializeField] float timeToDisableShotsEffects;
    public float _timeToDisableShotsEffects => timeToDisableShotsEffects;

    [SerializeField] float recoil;
    public float _recoil => recoil;

    [SerializeField] float recoilFactor;
    public float _recoilFactor => recoilFactor;

    [SerializeField] float[] stateBasedRecoil;
    public float[] _stateBasedRecoil => stateBasedRecoil;

    [SerializeField] float maxTracerRecoil;
    public float _maxTracerRecoil => maxTracerRecoil;

    [SerializeField] float smoothTimer;
    public float _smoothTimer => smoothTimer;
}
