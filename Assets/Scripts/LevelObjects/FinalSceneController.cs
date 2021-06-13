using PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalSceneController : MonoBehaviour
{
    public Image imageRef;

    public Sprite ending1;
    public Sprite ending2;
    public Sprite ending3;


    void Start()
    {
        StartCoroutine(FinalScene());   
    }

    IEnumerator FinalScene()
    {
        yield return new WaitForSeconds(3);

        Hub.Default.Publish(new FadeScreenMessage());
        yield return new WaitForSeconds(1);
        imageRef.sprite = ending2;

        Hub.Default.Publish(new RevealScreenMessage());
        yield return new WaitForSeconds(1);

        yield return new WaitForSeconds(3);

        Hub.Default.Publish(new FadeScreenMessage());
        yield return new WaitForSeconds(1);
        imageRef.sprite = ending3;
        imageRef.SetNativeSize();
        Hub.Default.Publish(new RevealScreenMessage());
    }

}
