using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] WeaponInfo weaponInfo;
    [SerializeField] GameObject magicLaser;
    Transform magicLaserSpawnPoint;
    Animator myAnimator;
    readonly int FIRE_HASH = Animator.StringToHash("Fire");
    
    private void Awake() {
        myAnimator = GetComponent<Animator>();
    }

    private void Start() {
        magicLaserSpawnPoint = this.transform.GetChild(1).transform;
    }
    public void Attack()
    {
        myAnimator.SetFloat("AnimSpeed", 1f/weaponInfo.weaponCooldown);
        myAnimator.SetTrigger(FIRE_HASH);
    }

    public void SpawnStaffProjectileAnimEvent()
    {
        GameObject newLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
        newLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }

    private void Update() {
        MouseFollowWithOfset();
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    private void MouseFollowWithOfset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        if(!PlayerController.Instance.TurnedLeft)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
    }
}
