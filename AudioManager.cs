using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip shot;
    [SerializeField] private float shotVolume;

    [Space(5)]
    [SerializeField] private AudioClip win;
    [SerializeField] private float winVolume;

    [Space(5)]
    [SerializeField] private AudioClip loose;
    [SerializeField] private float looseVolume;


    private void PlaySound(AudioClip clip, float volume)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }

    public void Play_ShotSound()
    {
        PlaySound(shot, shotVolume);
    }

    public void Play_Win()
    {
        PlaySound(win, winVolume);
    }

    public void Play_Loose()
    {
        PlaySound(loose, looseVolume);
    }
}
