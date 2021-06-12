using PubSub;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeManager : MonoBehaviour
{
    public Image fadeElement;
    public bool faded;
    public bool revealed;
    public float alphaTarget;
    public float fadeSpeed;
    public float currentFade = 1;

    private void Awake()
    {
        Hub.Default.Subscribe<FadeScreenMessage>(this, FadeMessageHandler);
        Hub.Default.Subscribe<RevealScreenMessage>(this, RevealMessageHandler);
        fadeElement.color = new Color(fadeElement.color.r, fadeElement.color.g, fadeElement.color.b, 1);
    }

    private void OnDestroy()
    {
        Hub.Default.Unsubscribe<FadeScreenMessage>(this, FadeMessageHandler);
        Hub.Default.Unsubscribe<RevealScreenMessage>(this, RevealMessageHandler);
    }

    private void Update()
    {
        currentFade = Mathf.MoveTowards(currentFade, alphaTarget, fadeSpeed * Time.deltaTime);
        fadeElement.color = new Color(fadeElement.color.r, fadeElement.color.g, fadeElement.color.b, currentFade);

        faded = currentFade == 1;
        revealed = currentFade == 0;
    }

    void FadeMessageHandler(FadeScreenMessage message)
    {
        FadeScreen();
    }

    void RevealMessageHandler(RevealScreenMessage message)
    {
        RevealScreen();
    }

    [Button]
    public void FadeScreen()
    {
        alphaTarget = 1;
    }

    [Button]
    public void RevealScreen()
    {
        alphaTarget = 0;
    }
}

public class FadeScreenMessage { }
public class RevealScreenMessage { }
