using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D body;

    public float moveSpeed = 20f;

    private bool isChasing = false;
    private Vector3 spawnPos;
    private bool hasSpawned = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasSpawned) {
            if (isChasing) {
                Vector3 targetDir = player.position - transform.position;
                body.AddForce(targetDir.normalized * moveSpeed);
            } else {
                Vector3 targetDir = spawnPos - transform.position;
                body.AddForce(targetDir.normalized * moveSpeed);
            }
        }
    }

    public void Spawn(Vector3 spawnPoint) {
        if (!hasSpawned) {
            spawnPos = new Vector3(spawnPoint.x, spawnPoint.y, 0f);
            transform.position = new Vector2(spawnPos.x, spawnPos.y);
            hasSpawned = true;
        }
        isChasing = true;
    }

    public void FallBack() {
        isChasing = false;
    }
}
