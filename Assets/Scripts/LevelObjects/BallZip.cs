using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallZip : MonoBehaviour
{
    public float zipSpeed = 5;
    public bool zipPlayer;
    public GameObject placeHolder;

    public void BeginZip()
    {
        StartCoroutine(ZipRoutine());
    }

    IEnumerator ZipRoutine()
    {
        var player = PlayerController.intance;
        player.gameObject.SetActive(false);
        var spawned = Instantiate(placeHolder, player.transform.position, Quaternion.identity);

        while(Vector2.Distance(player.transform.position, transform.position) > 0.05f)
        {
            var position = Vector2.MoveTowards(player.transform.position, transform.position, zipSpeed * Time.deltaTime);
            player.transform.position = position;
            spawned.transform.position = position;
            yield return null;
        }

        Destroy(spawned);
        player.gameObject.SetActive(true);
        player.parameters.gravity = 9;
    }
}
