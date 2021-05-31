using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public enum Character { bunny = 0, racoon = 1 }

    private Character selection = Character.bunny;

    public void SelectCharacter(int m) {
        selection = (Character) m;
    }
}
