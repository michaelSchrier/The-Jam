using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip titleTheme;
    public AudioClip chaseTheme;
    public AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        Hub.Default.Subscribe<AudioSetMessage>(this, SetAudio);
    }

    private void OnDestroy()
    {
        Hub.Default.Unsubscribe<AudioSetMessage>(this, SetAudio);
    }

    void SetAudio(AudioSetMessage message)
    {
        if(message.musicTheme == Music.Title)
        {
            source.clip = titleTheme;
        }
        if (message.musicTheme == Music.Chase)
        {
            source.clip = chaseTheme;
        }

        source.Play();
    }
}

public enum Music
{
    Title,
    Chase
}

public class AudioSetMessage
{
    public Music musicTheme;

    public AudioSetMessage(Music musicTheme)
    {
        this.musicTheme = musicTheme;
    }
}
