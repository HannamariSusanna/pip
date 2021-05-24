using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PickUp();
        }
    }

    private void PickUp() {
        Debug.Log("Power up jea");
        Destroy(gameObject);
    }
}
