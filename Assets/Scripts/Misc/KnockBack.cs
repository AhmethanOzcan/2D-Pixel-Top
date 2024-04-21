using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public bool GettingKnockedBack {get; private set;}
    [SerializeField] float knockBackTime = 0.2f;
    Rigidbody2D myRigidbody;
    // Start is called before the first frame update
    private void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockBackThrust)
    {
        GettingKnockedBack = true;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackThrust * myRigidbody.mass;
        myRigidbody.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        myRigidbody.velocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}
