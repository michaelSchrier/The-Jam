using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    public string sceneToChange;

    public void ChangeScene()
    {
        Hub.Default.Publish(new ChangeSceneMessage(sceneToChange)); 
        if(PlayerController.intance != null)
        {
            PlayerController.intance.gameObject.SetActive(false);
        }
    }
}
