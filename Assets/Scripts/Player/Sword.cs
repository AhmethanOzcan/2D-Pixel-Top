using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] GameObject slashAnimationPrefab;
    [SerializeField] Transform slashAnimationSpawnPos;
    [SerializeField] Transform weaponCollider;
    [SerializeField] float attackCooldown = 0.1f;
    GameObject slashAnimation;
    PlayerControls playerControls;
    Animator myAnimator;
    PlayerController playerController;
    ActiveWeapon activeWeapon;
    bool attackButtonDown, isAttacking = false;
    private void Awake() 
    {
        myAnimator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void Attack()
    {
        if(attackButtonDown && !isAttacking)
        {
            isAttacking = true;
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);
            slashAnimation = Instantiate(slashAnimationPrefab, slashAnimationSpawnPos.position, Quaternion.identity);
            slashAnimation.transform.parent = this.transform.parent;
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void DoneAttack()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnim()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180,0,0);
        slashAnimation.GetComponent<SpriteRenderer>().flipX = playerController.TurnedLeft;
    }

    public void SwingDownFlipAnim()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        slashAnimation.GetComponent<SpriteRenderer>().flipX = playerController.TurnedLeft;
    }

    // Update is called once per frame
    void Update()
    {
        MouseFollowWithOfset();
        Attack();
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void MouseFollowWithOfset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        if(!playerController.TurnedLeft)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }
}
