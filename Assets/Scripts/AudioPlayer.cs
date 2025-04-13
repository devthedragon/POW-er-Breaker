using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] sfx;
    [SerializeField] AudioSource musicSource;
    bool isLooping;

    public void PlayAudio(int index)
    {
        AudioSource.PlayClipAtPoint(sfx[index], transform.position);
    }

    public void PlayAudio(int index, Vector3 location)
    {
        AudioSource.PlayClipAtPoint(sfx[index], location);
    }

    public void SwitchMusic(int index)
    {
        musicSource = GameObject.Find("MusicController").GetComponent<AudioSource>();

        if (index >= 0)
        {
            musicSource.clip = sfx[index];
            musicSource.Play();
        }
        else 
        {
            musicSource.Pause();
        }
    }
}
