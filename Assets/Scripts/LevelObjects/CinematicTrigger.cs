using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    public List<AudioClip> sounds;
    private AudioSource audioSouce;

    private void Awake()
    {
        audioSouce = GetComponent<AudioSource>();
    }

    public void PlayScream()
    {
        if(sounds.Count > 0)
        {
            var count = sounds.Count;
            var random = Random.Range(0, count - 1);
            var chosenScream = sounds[random];

            audioSouce.clip = chosenScream;
            audioSouce.Play();
        }     

        Hub.Default.Publish(new CameraShakeMessage(6, 3, 2));
    }
}
