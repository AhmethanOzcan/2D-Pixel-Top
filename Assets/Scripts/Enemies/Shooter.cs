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
    [SerializeField] bool stagger = false;
    [Tooltip("Stagger has to be enabled for oscillate to work properly.")]
    [SerializeField] bool oscillate = false;
    bool isShooting = false;

    private void OnValidate() {
        if(oscillate) {stagger = true;}
        if(!oscillate) {stagger = false;}
        if(projectilesPerBurst < 1) {projectilesPerBurst = 1;}
        if(burstCount < 1) {burstCount = 1;}
        if(timeBetweenBurst < .1f) {timeBetweenBurst = .1f;}
        if(startingDistance < .1f) {startingDistance = .1f;}
        if(angleSpread == 0) {projectilesPerBurst = 1;}
        if(bulletSpeed <= 0) {bulletSpeed = 0.1f;}

    }


    public void Attack(){
        if(isShooting)
            return;
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;
        float targetAngle, startAngle, endAngle, currentAngle, halfAngleSpread, angleStep;
        float timeBetweenProjectiles = 0f;
        TargetConeOfInfluence(out targetAngle, out startAngle, out endAngle, out currentAngle, out halfAngleSpread, out angleStep);

        if(stagger)
        {
            timeBetweenBurst = timeBetweenBurst / projectilesPerBurst;
        }

        for (int i = 0; i < burstCount; i++)
        {
            if(!oscillate)
            {
                TargetConeOfInfluence(out targetAngle, out startAngle, out endAngle, out currentAngle, out halfAngleSpread, out angleStep);
            }
            if(oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out targetAngle, out startAngle, out endAngle, out currentAngle, out halfAngleSpread, out angleStep);
            }
            else if(oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }
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
                if(stagger)
                {
                    yield return new WaitForSeconds(timeBetweenProjectiles);
                }
            }

            currentAngle = startAngle;
            if(!stagger)
                yield return new WaitForSeconds(timeBetweenBurst);
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
