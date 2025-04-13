using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictorySequence : MonoBehaviour
{
    [SerializeField] SpriteSwitcher[] electriKitties;
    [SerializeField] AudioPlayer audioPlayer;

    void Start()
    {
        StartCoroutine(winSequence());
    }

    IEnumerator winSequence()
    {
        int loops = 0;

        for (int i = 0, n = electriKitties.Length; i < n; i++)
        {
            electriKitties[i].Initialize();
        }

        OverlayManager.om.canPause = false;

        audioPlayer.SwitchMusic(0);

        while (loops < 20)
        {
            for (int i = 0, n = electriKitties.Length; i < n; i++) 
            {
                electriKitties[i].SwitchSprite((i + loops) % 2);
            }

            yield return new WaitForSeconds(0.5f);
            loops++;
        }
        OverlayManager.om.WinScreen();
        while (true)
        {
            for (int i = 0, n = electriKitties.Length; i < n; i++)
            {
                electriKitties[i].SwitchSprite((i + loops) % 2);
            }

            yield return new WaitForSecondsRealtime(0.5f);
            loops++;
        }
    }
}
