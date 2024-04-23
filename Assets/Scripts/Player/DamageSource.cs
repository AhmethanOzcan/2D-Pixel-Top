using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    int damageAmount = 1;
    float knockbackAmount = 1f;
    private void Start() {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon; 
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
        knockbackAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().knockbackAmount;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDamage(damageAmount, knockbackAmount);
    }
}
