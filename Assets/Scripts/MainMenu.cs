using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        StartCoroutine(loadSceneWithDelay(SceneManager.GetActiveScene().buildIndex + 1, 0.6f));
    }

    public void Options() {
        int optionsIndex = SceneManager.sceneCountInBuildSettings - 1;
        StartCoroutine(loadSceneWithDelay(optionsIndex, 0.6f));
    }

    public IEnumerator loadSceneWithDelay(int index, float delay) {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SceneUtils.load(index));
    }
}
