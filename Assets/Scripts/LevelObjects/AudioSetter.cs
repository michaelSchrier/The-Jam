using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSetter : MonoBehaviour
{
    public Music music;

    public void ChangeAudio()
    {
        Hub.Default.Publish(new AudioSetMessage(music));
    }

}
