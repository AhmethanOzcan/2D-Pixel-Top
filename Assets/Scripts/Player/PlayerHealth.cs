using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] int maxHealth = 10;
    [SerializeField] float knockBackThrustAmount = 10f;
    [SerializeField] float immunitySeconds = 1f;
    // [SerializeField] GameObject deathVFXPrefab;
    int currentHealth;
    KnockBack knockback;
    Flash flash;
    Slider healthSlider;
    bool canTakeDamage = true;
    const string HEALTH_TEXT = "Health Slider";

    protected override void Awake() {
        base.Awake();
        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
    }

    private void Start() {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    public void TakeDamage(int damage, float knockBackAmount, Transform hitTransform)
    {
        if(!canTakeDamage)
        {
            return;
        }
        canTakeDamage = false;
        ScreenShakeManager.Instance.ShakeScreen();
        currentHealth -= damage;
        UpdateHealthSlider();
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(DetectDeathRoutine());
        knockback.GetKnockedBack(hitTransform, knockBackAmount* knockBackThrustAmount);
        StartCoroutine(ImmunityRoutine());   
    }

    public void HealPlayer(int healAmount)
    {
        if(currentHealth + healAmount < maxHealth)
            currentHealth += healAmount;
        else
            currentHealth = maxHealth;
        UpdateHealthSlider();
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
            currentHealth = 0;
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

    private void UpdateHealthSlider()
    {
        if(healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_TEXT).GetComponent<Slider>();
            healthSlider.maxValue = maxHealth;
        }
        healthSlider.value = currentHealth;
    }
}
