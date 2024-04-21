using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    [SerializeField] string sceneTransitionName;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<PlayerController>())
        {
            StartCoroutine(TransitionRoutine());
        }
    }

    private IEnumerator TransitionRoutine()
    {
        UIFade.Instance.FadeToBlack();
        yield return new WaitForSeconds(UIFade.Instance.FadeSpeed);
        SceneManager.LoadScene(sceneToLoad);
        SceneManagement.Instance.SetTransitionName(sceneTransitionName);
    }
}
