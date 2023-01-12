using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXplayer : MonoBehaviour
{
    public AudioSource Ambience;
    public AudioSource AmbienceToy;
    public AudioSource Clunk;
    public AudioSource Skid;
    public AudioSource Starting;
    public AudioSource Starting2;
    public AudioSource Running;
    public AudioSource Running2;


    public void PlayAmbience()
    {
        Ambience.Play();
    }

    public void PlayAmbienceToy()
    {
        AmbienceToy.Play();
    }
    public void PlayClunk()
    {
        Clunk.Play();
    }
    public void PlaySkid()
    {
        Skid.Play();
    }
    public void PlayStarting()
    {
        Starting.Play();
    }
    public void PlayStarting2()
    {
        Starting2.Play();
    }
    public void PlayRunning()
    {
        Running.Play();
    }
    public void PlayRunning2()
    {
        Running2.Play();
    }

    public void Start()
    {
       //PlayAmbience();
    }


}
