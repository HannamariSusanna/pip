using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI tm;
    private float millisElapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        millisElapsed += Time.deltaTime;
        float minutes = Mathf.FloorToInt(millisElapsed / 60);
        float seconds = Mathf.FloorToInt(millisElapsed % 60);
        tm.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
