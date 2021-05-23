using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometUpdater : MonoBehaviour
{
    public Vector2 direction;
    public float moveSpeed = 20.0f;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        body.AddForce(direction * moveSpeed);
    }
    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
