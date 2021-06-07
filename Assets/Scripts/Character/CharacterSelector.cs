using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public enum Character { bunny = 0, racoon = 1 }

    public static Character selection = Character.racoon;

    public void SelectCharacter(int m) {
        CharacterSelector.selection = (Character) m;
    }
}
