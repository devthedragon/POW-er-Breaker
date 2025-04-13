using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager om;

    [SerializeField] GameObject[] overlays;
    bool isPaused = false;
    float prevSpeed = 1;

    [HideInInspector] public bool canPause = true;

    // Start is called before the first frame update
    void Start()
    {
        if (om == null)
        {
            om = this;
        }
        else 
        {
            Destroy(this);
        }
        FadeIn(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (canPause)
        {
            if (isPaused)
            {
                PauseScreen(false);
            }
            else
            {
                PauseScreen(true);
            }
        }
    }

    public void Pause()
    {
        prevSpeed = Time.timeScale;
        Time.timeScale = 0;
        PipeManager.pm.Pause();
        isPaused = true;
    }

    public void Unpause()
    {
        Time.timeScale = prevSpeed;
        PipeManager.pm.Unpause();
        isPaused = false;
    }

    public void PauseScreen(bool isActive)
    {
        overlays[0].SetActive(isActive);

        if (isActive)
        {
            Pause();
        }
        else
        {
            Unpause();
        }
    }

    public void DeathScreen()
    {
        overlays[1].SetActive(true);
        Pause();
        canPause = false;
    }

    public void WinScreen()
    {
        overlays[2].SetActive(true);
        Pause();
        canPause = false;
    }

    public void FadeIn(float time) 
    {
        StartCoroutine(Fade(false, time));
    }

    public void FadeOut(float time)
    {
        StartCoroutine(Fade(true, time));
    }

    void SetFade(float fadeAmount) 
    {
        Image tempImage = overlays[3].GetComponent<Image>();
        Color tempColor = tempImage.color;
        tempColor.a = fadeAmount;
        tempImage.color = tempColor;
        OverlayManager.om.canPause = true;
    }

    IEnumerator Fade(bool fadeOut, float runTime)
    {
        float timer = 0;
        while (timer < runTime)
        {
            timer += Time.unscaledDeltaTime;
            SetFade(Mathf.Lerp(1 - (1 * Convert.ToInt32(fadeOut)), 1 * Convert.ToInt32(fadeOut), timer/runTime));
            
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.5f);
    }
}
