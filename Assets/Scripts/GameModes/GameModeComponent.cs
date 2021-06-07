using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameModeComponent : MonoBehaviour {
    public GameModeSelector.GameModes gameMode;
    public GameObject[] characterPrefabs;
    public GameObject player;
    private GameMode selection;

    public void Awake() {
        InitializeCharacter();
    }
    
    public void OnValidate() {
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
    }
    public GameMode GetGameMode() {
        return selection;
    }

    private void InitializeCharacter() {
        GameObject playerPrefab = characterPrefabs[(int)CharacterSelector.selection];
        GameObject p = Instantiate(playerPrefab, player.transform.parent.transform);
        player.transform.SetParent(p.transform);
        selection.SetPlayer(p.GetComponent<Player>());
    }
}