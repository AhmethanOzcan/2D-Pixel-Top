using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum State{
        Roaming,
        Attacking
    }
    [SerializeField] float roamChangeDirFloat = 2f;
    [SerializeField] int collusionDamage = 3;
    [SerializeField] float collusionKnockback = 2f;
    [SerializeField] float shootingRange = 5f;
    [SerializeField] MonoBehaviour enemyType;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] bool stopWhileAttacking = false;
    float timeRoaming = 0f;
    bool canAttack = true;
    State state;
    EnemyPathFinding enemyPathFinding;
    Vector2 roamPosition;
    private void Awake() {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Roaming;
    }

    private void Start() {
        roamPosition =  GetRoamingPosition();
    }

    private void Update() {
        MovementStateControl();   
    }

    private void MovementStateControl()
    {
        switch(state)
        {
            
            case State.Roaming:
                Roaming();
            break;

            case State.Attacking:
                Attacking();
            break;

            default:
            break;
        }
    }

    private void Attacking()
    {
        if(Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > shootingRange)
        {
            state = State.Roaming;
        }

        if(shootingRange == 0 || !canAttack)
        {
            return;
        }
        canAttack = false;
        (enemyType as IEnemy).Attack();

        if(stopWhileAttacking)
        {
            enemyPathFinding.StopMoving();
        }
        else
        {
            enemyPathFinding.MoveTo(roamPosition);
        }

        StartCoroutine(AttackCooldownRoutine());
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;
        enemyPathFinding.MoveTo(roamPosition);
        if(Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < shootingRange)
        {
            state = State.Attacking;
        }


        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }


    Vector2 GetRoamingPosition(){
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        if(player)
        {
            player.TakeDamage(collusionDamage, collusionKnockback, this.transform);
        }
    }
}
