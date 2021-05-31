using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneUtils {
    public static IEnumerator load(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}