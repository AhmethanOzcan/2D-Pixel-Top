using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool IsDead {get; private set;}
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
    const string TOWN_TEXT = "Town";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake() {
        base.Awake();
        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
    }

    private void Start() {
        IsDead = false;
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
        if(currentHealth <= 0 && !IsDead)
        {
            currentHealth = 0;
            IsDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadRoutine());            
            
        }
    }

    private IEnumerator DeathLoadRoutine()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        PlayerStamina.Instance.ReplenishStaminaOnDeath();
        SceneManager.LoadScene(TOWN_TEXT);
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
