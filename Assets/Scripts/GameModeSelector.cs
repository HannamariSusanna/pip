using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeSelector : MonoBehaviour
{
    public enum GameMode { standard = 0, infinite = 1 }

    private GameMode selection;

    public void SelectGameMode(int m) {
        selection = (GameMode) m;
        StartCoroutine(SceneUtils.load(SceneManager.GetActiveScene().buildIndex + 1 + m));
    }
}
