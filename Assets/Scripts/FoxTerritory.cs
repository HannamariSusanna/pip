using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxTerritory : MonoBehaviour
{
    public GameObject fox;
    public Vector3 spawnPoint;

    private Fox foxScript;

    public void Start() {
        foxScript = fox.GetComponent(typeof(Fox)) as Fox;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            foxScript.Spawn(transform.TransformPoint(spawnPoint));
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            foxScript.FallBack();
        }
    }
}
