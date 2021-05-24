using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometUpdater : MonoBehaviour
{
    public Transform player;
    public Vector2 direction;
    public float moveSpeed = 20.0f;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start() {
        body = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
    }

    void Update() {
        if (Vector3.Distance(transform.position, player.position) > 25f) {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        body.AddForce(direction * moveSpeed);
    }
}
