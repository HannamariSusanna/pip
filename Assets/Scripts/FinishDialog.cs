using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDialog : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timeBonusText;
    public TMPro.TextMeshProUGUI carrotBonusText;
    public TMPro.TextMeshProUGUI collisionPenaltyText;
    public TMPro.TextMeshProUGUI totalText;
    public Timer timer;
    public DistancePointsCalculator distanceCalculator;
    public float duration = 0.5f;

    public GameModeComponent gameModeComponent;

    public void OnEnable() {
        PauseGame();
        StartCoroutine(CalculatePoints());
    }

    public void OnDisable() {
        ContinueGame();
    }

    public void FinishLevel() {
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void Retry() {
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void PauseGame() {
        Time.timeScale = 0;
    } 
    private void ContinueGame() {
        Time.timeScale = 1;
    }

    private IEnumerator CalculatePoints() {
        GameMode gameMode = gameModeComponent.GetGameMode();
        
        int timeBonus = 0;
        if (timer) {
            timeBonus = Mathf.Max((300 - timer.GetSeconds()) * 55, 0);
        }
        if (distanceCalculator) {
            timeBonus = distanceCalculator.GetPoints();
        }
        int carrotBonus = gameMode.CarrotBonus();
        int collisionPenalty = gameMode.CollisionPenalty();
        var timeBonusRoutine = CountTo(timeBonus, timeBonusText);
        var carrotBonusRoutine = CountTo(carrotBonus, carrotBonusText);
        var collisionPenaltyRoutine = CountTo(collisionPenalty, collisionPenaltyText);
        int total = timeBonus + carrotBonus + collisionPenalty;
        var totalRoutine = CountTo(total, totalText);
        yield return StartCoroutine(timeBonusRoutine);
        yield return StartCoroutine(carrotBonusRoutine);
        yield return StartCoroutine(collisionPenaltyRoutine);
        yield return StartCoroutine(totalRoutine);
    }

    IEnumerator CountTo(int target, TMPro.TextMeshProUGUI field) {
        int start = 0;
        for (float timer = 0; timer < duration; timer += Time.unscaledDeltaTime) {
            float progress = timer / duration;
            int value = (int)Mathf.Lerp(start, target, progress);
            field.text = value.ToString();
            yield return null;
        }
        field.text = target.ToString();
     }
}
