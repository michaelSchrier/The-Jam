using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private void Awake()
    {
        Hub.Default.Subscribe<ChangeSceneMessage>(this, ChangeSceneHander);
    }

    private void OnDestroy()
    {
        Hub.Default.Unsubscribe<ChangeSceneMessage>(this, ChangeSceneHander);
    }

    void ChangeSceneHander(ChangeSceneMessage message)
    {
        StartCoroutine(ChangeScene(message.SceneName));
    }

    IEnumerator ChangeScene(string sceneName)
    {
        Hub.Default.Publish(new FadeScreenMessage());

        yield return new WaitForSeconds(1);

        var operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }

        Hub.Default.Publish(new RevealScreenMessage());
    }
}

public class ChangeSceneMessage
{
    public ChangeSceneMessage(string sceneName)
    {
        SceneName = sceneName;
    }

    public string SceneName { get; }
}
