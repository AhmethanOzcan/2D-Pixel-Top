using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float moveSpeed = 22f;
    [SerializeField] GameObject particleOnHitVFX;
    WeaponInfo weaponInfo;
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
        if(Vector3.Distance(transform.position, startPosition) >= weaponInfo.weaponRange)
        {
            Instantiate(particleOnHitVFX,transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }

    public void UpdateWeaponInfo(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
    }

    private void MoveProjectile()
    {
        this.transform.Translate(Vector3.right*(moveSpeed*Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        if(!other.isTrigger && (enemyHealth || indestructible))
        {
            Instantiate(particleOnHitVFX,transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }
}
