using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    Rigidbody2D myRigibbody;
    Vector2 moveDir;
    KnockBack knockBack;
    void Start()
    {
        myRigibbody = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
    }

    void FixedUpdate()
    {
        if(!knockBack.GettingKnockedBack)
            myRigibbody.MovePosition(myRigibbody.position + moveDir * (movementSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDir = targetPosition;
    }
}
