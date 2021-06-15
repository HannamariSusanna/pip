using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public GameObject sensitivity;

    void Start() {
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        this.sensitivity.GetComponent<Slider>().value = sensitivity;
    }

    public void Back() {
        int mainMenuIndex = 0;
        StartCoroutine(SceneUtils.load(mainMenuIndex));
    }

    public void SetSensitivity() {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity.GetComponent<Slider>().value);
    }
}
