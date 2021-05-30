using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject finishDialog;

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            finishDialog.SetActive(true);
        }
    }
}
