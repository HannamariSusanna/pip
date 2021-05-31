using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseDialog : MonoBehaviour
{

    private bool isPaused = false;

    public void FinishLevel() {
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
        ContinueGame();
    }

    public void Retry() {
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ContinueGame();
    }

    public bool IsPaused() {
        return isPaused;
    }

    public void PauseGame() {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    } 
    public void ContinueGame() {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
}
