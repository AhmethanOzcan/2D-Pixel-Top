using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUp : MonoBehaviour
{
    enum PickUpType
    {
        GoldCoin,
        StaminaGlobe,
        HealthGlobe,
    }

    [SerializeField] PickUpType pickUpType;
    [SerializeField] float magnatiseDistance = 5f;
    [SerializeField] float accelarationRate = 2f;
    [SerializeField] AnimationCurve animCurve;
    [SerializeField] float heightY = 1.5f;
    [SerializeField] float popDuration = 1f;
    float moveSpeed = 0f;
    Rigidbody2D myRigidBody;
    Vector3 moveDir;

    private void Awake() {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update() {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        if(Vector3.Distance(playerPos, this.transform.position) < magnatiseDistance)
        {
            moveDir = (playerPos-this.transform.position).normalized;
            moveSpeed += accelarationRate;
        }
        else
        {
            moveDir = Vector3.zero;
            moveSpeed = 0;
        }
    }

    private void FixedUpdate() {
        myRigidBody.velocity = moveDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<PlayerController>())
        {
            DetectPickupType();
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPoint = transform.position;
        Vector2 endPoint = transform.position + new Vector3 (Random.Range(-2,2), Random.Range(-2,2), 0);
        float timePassed = 0f;
        while(timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);

            yield return null;
        }
    }

    private void DetectPickupType()
    {
        switch(pickUpType)
        {
            case PickUpType.GoldCoin:
                EconomyManager.Instance.UpdateCurrentGold(1);
                break;
            case PickUpType.StaminaGlobe:
                PlayerStamina.Instance.RefreshStamina();
                break;
            case PickUpType.HealthGlobe:
                int randomAmount = Random.Range(1,4);
                PlayerHealth.Instance.HealPlayer(randomAmount);
                break;
            default:
                break;
        }
    }
}
