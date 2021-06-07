using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometUpdater : MonoBehaviour
{
    public Transform player;
    public Vector2 direction;
    public float moveSpeed = 20.0f;
    public Rigidbody2D body;

    void OnEnable() {
        Vector3 targetDir = player.position - transform.position;
        float angle = (Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg) + 135;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        direction = targetDir.normalized;
    }

    void Update() {
        if (Vector3.Distance(transform.position, player.position) > 25f) {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        body.AddForce(direction * moveSpeed);
    }

    void OnCollisionEnter2D() {
        RotateToMovementDir();
    }
    void OnCollisionStay2D() {
        RotateToMovementDir();
    }
    
    private void RotateToMovementDir() {
        Vector3 dir = new Vector3(body.velocity.normalized.x, body.velocity.normalized.y, 0f);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 135;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
