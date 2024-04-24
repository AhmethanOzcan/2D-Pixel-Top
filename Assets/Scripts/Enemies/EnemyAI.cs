using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum State{
        Roaming
    }
    [SerializeField] float roamChangeDirFloat = 2f;
    [SerializeField] int collusionDamage = 3;
    [SerializeField] float collusionKnockback = 2f;
    State state;
    EnemyPathFinding enemyPathFinding;

    private void Awake() {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Roaming;
    }

    private void Start() {
        StartCoroutine(RoamingRoutine());
    }

    IEnumerator RoamingRoutine(){
        while(state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathFinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(roamChangeDirFloat);
        }
    }

    Vector2 GetRoamingPosition(){
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
