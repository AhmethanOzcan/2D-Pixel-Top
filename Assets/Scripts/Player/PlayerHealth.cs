using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 10;
    [SerializeField] float knockBackThrustAmount = 10f;
    [SerializeField] float immunitySeconds = 1f;
    // [SerializeField] GameObject deathVFXPrefab;
    int currentHealth;
    KnockBack knockback;
    Flash flash;
    bool canTakeDamage = true;

    private void Awake() {
        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
    }

    private void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, float knockBackAmount, Transform hitTransform)
    {
        if(!canTakeDamage)
        {
            return;
        }
        canTakeDamage = false;
        currentHealth -= damage;
        knockback.GetKnockedBack(hitTransform, knockBackAmount* knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(DetectDeathRoutine());
        StartCoroutine(ImmunityRoutine());
        
        
    }

    IEnumerator DetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    private void DetectDeath()
    {
        if(currentHealth <= 0)
        {
            // Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            // Destroy(gameObject);
            Debug.Log("You are dead!");
        }
    }

    private IEnumerator ImmunityRoutine()
    {
        yield return new WaitForSeconds(immunitySeconds);
        canTakeDamage = true;
    }
}
