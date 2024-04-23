using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] float laserGrowTime = 2f;
    bool isGrowing = true;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D myCollider;
    float laserRange;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<CapsuleCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        LaserFaceMouse();
        
    }

    public void UpdateLaserRange(float laserRangeValue)
    {
        laserRange = laserRangeValue;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;
        while(spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float linerT = timePassed / laserGrowTime;
            Vector2 newSize = new Vector2(Mathf.Lerp(1f, laserRange, linerT),1f);
            spriteRenderer.size = newSize;
            newSize.y = myCollider.size.y;
            myCollider.size = newSize;
            Vector2 newColliderOffset = new Vector2((Mathf.Lerp(1f, laserRange, linerT))/2, myCollider.offset.y);
            myCollider.offset = newColliderOffset; 
            yield return null;
        }
        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    private void LaserFaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = transform.position - mousePosition;
        transform.right = -direction;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Indestructible>() && !other.isTrigger)
        {
            isGrowing = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
