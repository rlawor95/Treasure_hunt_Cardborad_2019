using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioSource BGM;
    public AudioSource CorrectSnd;
    public AudioSource IncorrectSnd;
    public AudioSource TeleportSnd;

    void Start()
    {
        if (instance == null)
            instance = this;

        BGM.Play();
    }

    public void ToggleBGM(bool b)
    {
        if (b)
            BGM.Stop();
        else
            BGM.Play();
    }

    public void PlayCorrectSound()
    {
        CorrectSnd.Play();
    }

    public void PlayInCorrectSound()
    {
        IncorrectSnd.Play();
    }

    public void PlayTeleporSound()
    {
        TeleportSnd.Play();
    }

}
