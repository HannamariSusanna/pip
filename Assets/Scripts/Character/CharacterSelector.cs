using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    public enum Character { bunny = 0, red = 1, racoon = 2 }

    public static Character selection = Character.bunny;
    
    public GameObject[] characterPrefabs;
    public Sprite[] characters;
    public GameObject selectionImage;
    public TMPro.TextMeshProUGUI abilityText;
    public Image speed;
    public Image agility;
    public Image endurance;

    private static float animationDuration = 0.5f;
    private static float maxSpeed = 3f;
    private static float maxHealth = 10f;
    private static float maxDrag = 3f;

    void Start() {
        SelectCharacter((int) selection);
    }

    void Update() {
         if (Input.GetKeyDown(KeyCode.Escape)) {
             StartCoroutine(SceneUtils.load(SceneManager.GetActiveScene().buildIndex - 1));
         }
    }

    public void SelectCharacter(int m) {
        CharacterSelector.selection = (Character) m;
        selectionImage.GetComponent<Image>().sprite = characters[m];
        Player player = characterPrefabs[m].GetComponent<Player>();

        var speedCoroutine = ScaleTo(player.CalculateTerminalVelocity() / maxSpeed, speed.rectTransform);
        StartCoroutine(speedCoroutine);
        var enduranceCoroutine = ScaleTo(player.maxHealth / maxHealth, endurance.rectTransform);
        StartCoroutine(enduranceCoroutine);
        var agilityCoroutine = ScaleTo(player.body.drag / maxDrag, agility.rectTransform);
        StartCoroutine(agilityCoroutine);

        abilityText.text = player.ability.GetName();
    }

    IEnumerator ScaleTo(float target, RectTransform transform) {
        Vector3 initialScale = transform.localScale;
 
        for(float time = 0; time < animationDuration * 2; time += Time.unscaledDeltaTime) {
            float progress = time / animationDuration;
            transform.localScale = new Vector2(Mathf.Lerp(initialScale.x, target, progress), 1f);
            yield return null;
        }
        transform.localScale = new Vector2(target, 1f);
     }
}
