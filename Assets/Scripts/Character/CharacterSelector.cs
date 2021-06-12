using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public enum Character { bunny = 0, red = 1, racoon = 2 }

    public static Character selection = Character.racoon;
    
    public Sprite[] sprites;
    public GameObject selectionImage;

    public void SelectCharacter(int m) {
        CharacterSelector.selection = (Character) m;
        selectionImage.GetComponent<Image>().sprite = sprites[m];
    }
}
