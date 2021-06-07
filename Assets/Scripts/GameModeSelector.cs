using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeSelector : MonoBehaviour
{
    public enum GameModes { standard = 0, infinite = 1 }

    public void SelectGameMode(int m) {
        StartCoroutine(SceneUtils.load(SceneManager.GetActiveScene().buildIndex + 1 + m));
    }
}
