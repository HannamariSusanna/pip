using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Carrot : MonoBehaviour
{
    private Action<Vector2Int> callback;
    private Vector2Int callbackParam;

    public void SetPickupCallback(Action<Vector2Int> action, Vector2Int coord) {
        this.callback = action;
        this.callbackParam = coord;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (callback != null) {
                callback(callbackParam);
            } else {
                PickUp();
            }
        }
    }

    private void PickUp() {
        Destroy(gameObject);
    }
}
