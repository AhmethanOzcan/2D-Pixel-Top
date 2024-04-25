using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float moveSpeed = 22f;
    [SerializeField] GameObject particleOnHitVFX;
    [SerializeField] float projectileRange = 10f;
    [SerializeField] bool isEnemyProjectile = false;
    Vector3 startPosition;
    private void Start() {
        startPosition = transform.position;
    }
    private void Update() {
        
        DetectDistance();
        MoveProjectile();
    }

    private void DetectDistance()
    {
        if(Vector3.Distance(transform.position, startPosition) >= projectileRange)
        {
            Instantiate(particleOnHitVFX,transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }


    private void MoveProjectile()
    {
        this.transform.Translate(Vector3.right*(moveSpeed*Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        if(!other.isTrigger)
        {
            if((player && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile) || indestructible)
            {
                player?.TakeDamage(3,5f, this.transform);
                Instantiate(particleOnHitVFX,transform.position,transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
