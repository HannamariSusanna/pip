using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float millisElapsed = 0;

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
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
