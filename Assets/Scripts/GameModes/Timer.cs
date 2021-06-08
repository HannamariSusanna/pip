using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour, PointsCalculator
{
    public TMPro.TextMeshProUGUI tm;
    private float secondsElapsed = 0;

    // Update is called once per frame
    void Update()
    {
        secondsElapsed += Time.deltaTime;
        float minutes = Mathf.FloorToInt(secondsElapsed / 60);
        float seconds = Mathf.FloorToInt(secondsElapsed % 60);
        tm.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public int GetSeconds() {
        return Mathf.RoundToInt(secondsElapsed);
    }

    public int GetGameModePoints() {
        return Mathf.Max((300 - GetSeconds()) * 55, 0);
    }
}
