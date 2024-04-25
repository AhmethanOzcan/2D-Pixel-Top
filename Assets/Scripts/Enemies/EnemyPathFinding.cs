using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    Rigidbody2D myRigibbody;
    Vector2 moveDir;
    KnockBack knockBack;
    SpriteRenderer spriteRenderer;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myRigibbody = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
    }

    void FixedUpdate()
    {
        if(knockBack.GettingKnockedBack)
            return;

        myRigibbody.MovePosition(myRigibbody.position + moveDir * (movementSpeed * Time.fixedDeltaTime));
        if(moveDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition;
    }

    public void StopMoving()
    {
        moveDir = Vector3.zero;
    }
}
