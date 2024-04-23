using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] GameObject slashAnimationPrefab;
    Transform slashAnimationSpawnPos;
    Transform weaponCollider;
    [SerializeField] WeaponInfo weaponInfo;
    GameObject slashAnimation;
    
    Animator myAnimator;
    private void Awake() 
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Start() {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimationSpawnPos = PlayerController.Instance.GetSlashAniSpawn();
    }
    

    public void Attack()
    {
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);
        slashAnimation = Instantiate(slashAnimationPrefab, slashAnimationSpawnPos.position, Quaternion.identity);
        slashAnimation.transform.parent = this.transform.parent;
    }

    private void DoneAttack()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnim()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180,0,0);
        slashAnimation.GetComponent<SpriteRenderer>().flipX = PlayerController.Instance.TurnedLeft;
    }

    public void SwingDownFlipAnim()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        slashAnimation.GetComponent<SpriteRenderer>().flipX = PlayerController.Instance.TurnedLeft;
    }

    // Update is called once per frame
    void Update()
    {
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
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }
}
