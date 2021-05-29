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
            PauseGame();
        }
    }

    public void FinishLevel() {
        finishDialog.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        ContinueGame();
    }

    public void Retry() {
        finishDialog.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ContinueGame();
    }

    private void PauseGame() {
        Time.timeScale = 0;
    } 
    private void ContinueGame() {
        Time.timeScale = 1;
    }
}
