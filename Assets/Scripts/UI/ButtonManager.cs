using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void LoadLevel(string lvlToLoad)
    {
        if (OverlayManager.om != null)
        {
            OverlayManager.om.Unpause();
        }
        SceneManager.LoadScene(lvlToLoad, LoadSceneMode.Single);
    }

    public void ReloadLevel() 
    {
        OverlayManager.om.Unpause();
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
