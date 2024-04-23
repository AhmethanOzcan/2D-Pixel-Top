using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool TurnedLeft{get{return turnedLeft;}}
    [SerializeField] float movementSpeed = 5f;
    float startingMovementSpeed;
    [SerializeField] float dashSpeed = 4f;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float dashCD = 1f;
    [SerializeField] TrailRenderer myTrailRenderer;
    [SerializeField] private Transform WeaponCollider;
    [SerializeField] private Transform SlashAniSpawn;
    PlayerControls playerControls;
    Vector2 movement;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;
    bool turnedLeft = false;
    bool dashing = false;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake(); 
        playerControls = new PlayerControls();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
        startingMovementSpeed = movementSpeed;

    }

    private void OnEnable() {
        playerControls.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        AdjustFacedWay();
        TakePlayerInput();
        Move();
    }

    private void AdjustFacedWay()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        if(playerPos.x <= mousePos.x )
        {
            turnedLeft = false;
        }
        else
        {
            turnedLeft = true;
        }
        mySpriteRenderer.flipX = TurnedLeft;
    }

    private void Move()
    {
        myRigidbody.MovePosition(myRigidbody.position + movement * (movementSpeed * Time.fixedDeltaTime));
    }

    private void TakePlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Dash()
    {
        if(!dashing)
        {
            dashing = true;
            myTrailRenderer.emitting = true;
            movementSpeed *= dashSpeed;
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        movementSpeed = startingMovementSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        dashing = false;
    }

    public Transform GetWeaponCollider()
    {
        return WeaponCollider;
    }

    public Transform GetSlashAniSpawn()
    {
        return SlashAniSpawn;
    }
}
