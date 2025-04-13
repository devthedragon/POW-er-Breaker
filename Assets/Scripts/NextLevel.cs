using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] string levelToLoad;

    void OnTriggerEnter(Collider collision) 
    {
        if (collision.gameObject.GetComponent<PlayerHealth>()) 
        {
            StartCoroutine(LoadLevelSequence());
        }
    }

    IEnumerator LoadLevelSequence()
    {
        OverlayManager.om.canPause = false;
        OverlayManager.om.FadeOut(1);
        yield return new WaitForSecondsRealtime(1);

        SceneManager.LoadScene(levelToLoad);
    }
}
