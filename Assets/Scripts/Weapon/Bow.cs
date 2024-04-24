using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] WeaponInfo weaponInfo;
    [SerializeField] GameObject arrowPrefab;
    Transform arrowSpawnPoint;
    Animator myAnimator;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private void Awake() {
        myAnimator = GetComponent<Animator>();
    }

    private void Start() {
        arrowSpawnPoint = this.transform.GetChild(0).transform;
    }
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void Attack()
    {
        myAnimator.SetFloat("AnimSpeed", 1f/weaponInfo.weaponCooldown);
        myAnimator.SetTrigger(FIRE_HASH);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);  
        newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);
    }   
}
