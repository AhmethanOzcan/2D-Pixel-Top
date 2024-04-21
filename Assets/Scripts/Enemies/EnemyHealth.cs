using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int fullHealth = 10;
    [SerializeField] GameObject deathVFXPrefab;
    [SerializeField] float knockBackThrust = 1f;
    int currentHealth;

    private KnockBack knockBack;
    private Flash flash;

    private void Start() {
        currentHealth = fullHealth;
        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
    }

    public void TakeDamage(int damage, float knockBackAmount)
    {
        currentHealth -= damage;
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackAmount* knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(DetectDeathRoutine());
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
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
