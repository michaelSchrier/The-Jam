using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TheBall : SerializedMonoBehaviour
{
    Rigidbody2D rb2d;
    CircleCollider2D cc2d;
    public GameObject creator;
    public bool returning;
    public float returnSpeed = 20;
    public float returnTime = 2;
    public float counter;
    public LineRenderer lineRenderer;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        cc2d = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (returning)
        {
            transform.position = Vector2.MoveTowards(transform.position, creator.transform.position, returnSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, creator.transform.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }

        if (counter > returnTime || (Input.GetMouseButton(1) && counter > 0.1f))
        {
            Return();
        }
        else
        {
            counter += Time.deltaTime;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, creator.transform.position);
    }

    [Button]
    public void Launch(Vector2 launchDirection)
    {
        rb2d.velocity = launchDirection;
    }

    public void Return()
    {
        returning = true;
        cc2d.enabled = false;
        rb2d.velocity = Vector2.zero;
        rb2d.bodyType = RigidbodyType2D.Kinematic;
    }
}
