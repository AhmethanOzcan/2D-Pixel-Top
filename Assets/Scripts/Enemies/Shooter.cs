using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] int burstCount = 1;
    [SerializeField] float timeBetweenBurst = .3f;
    [SerializeField] int projectilesPerBurst = 1;
    [SerializeField][Range(0,359)] float angleSpread =0;
    [SerializeField] float startingDistance = 0.1f;
    bool isShooting = false;


    public void Attack(){
        if(isShooting)
            return;
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;
        float targetAngle, startAngle, endAngle, currentAngle, halfAngleSpread, angleStep;
        TargetConeOfInfluence(out targetAngle, out startAngle, out endAngle, out currentAngle, out halfAngleSpread, out angleStep);

        

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateProjectileSpeed(bulletSpeed);
                }

                currentAngle += angleStep;
            }

            currentAngle = startAngle;
            yield return new WaitForSeconds(timeBetweenBurst);
            TargetConeOfInfluence(out targetAngle, out startAngle, out endAngle, out currentAngle, out halfAngleSpread, out angleStep);
        }
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float targetAngle, out float startAngle, out float endAngle, out float currentAngle, out float halfAngleSpread, out float angleStep)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        halfAngleSpread = 0f;
        angleStep = 0f;

        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector2 pos = new Vector2(x,y);

        return pos;
    }
}
