using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameModeComponent : MonoBehaviour {
    public GameModeSelector.GameModes gameMode;
    public GameObject[] characterPrefabs;
    public GameObject player;
    public FinishDialog finishDialog;
    public GameObject pointsCalculator;
    public int carrotBonus = 100;
    public int collisionPenalty = -100;
    public bool collisionsAllowedAfterHealthLoss = false;

    private GameMode selection;

    public void Start() {
        InitializeGameMode();
        InitializeCharacter();
    }
    
    public void OnValidate() {
        InitializeGameMode();
    }
    public GameMode GetGameMode() {
        return selection;
    }

    private void InitializeGameMode() {
        switch (gameMode) {
            case GameModeSelector.GameModes.standard:
                selection = new StandardGameMode();
                break;
            case GameModeSelector.GameModes.infinite:
                selection = new InfiniteGameMode();
                break;
            default:
                selection = new StandardGameMode();
                break;
        }

        selection.SetFinishDialog(finishDialog);
        selection.SetCarrotBonus(carrotBonus);
        selection.SetCollisionPenalty(collisionPenalty);
        selection.SetCollisionsAllowedAfterHealthLoss(collisionsAllowedAfterHealthLoss);
        selection.SetPointsCalculator(pointsCalculator);
    }

    private void InitializeCharacter() {
        int selected = (int)CharacterSelector.selection;
        GameObject playerPrefab = characterPrefabs[(int)CharacterSelector.selection];
        GameObject p = Instantiate(playerPrefab, player.transform.parent.transform);
        player.transform.SetParent(p.transform);
        Player playerScript = p.GetComponent<Player>();
        selection.SetPlayer(playerScript);
        playerScript.SetGameMode(selection);
    }
}