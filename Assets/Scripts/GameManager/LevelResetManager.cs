using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelResetManager : MonoBehaviour
{
    private void Awake()
    {
        Hub.Default.Subscribe<PlayerDeathMessage>(this, ResetLevelHandler);
    }

    private void OnDestroy()
    {
        Hub.Default.Unsubscribe<PlayerDeathMessage>(this, ResetLevelHandler);
    }

    void ResetLevelHandler(PlayerDeathMessage message)
    {
        StartCoroutine(ResetLevel());
    }

    IEnumerator ResetLevel()
    {
        Hub.Default.Publish(new FadeScreenMessage());

        yield return new WaitForSeconds(1);

        var scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);

        Hub.Default.Publish(new RevealScreenMessage());
    }

}
