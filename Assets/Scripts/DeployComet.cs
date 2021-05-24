using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployComet : MonoBehaviour
{
    public GameObject[] cometPrefabs;
    public Transform player;
    private Vector2 screenBounds;
    private System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        StartCoroutine(CometWave());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnComet() {
        int randomCometIndex = random.Next(0, cometPrefabs.Length);
        GameObject c = Instantiate(cometPrefabs[randomCometIndex]) as GameObject;
        c.transform.position = player.position + player.TransformDirection(new Vector2(screenBounds.x * 1.5f, screenBounds.y * 1f));
        c.transform.eulerAngles = player.eulerAngles;

        CometUpdater cometUpdater = c.GetComponent(typeof(CometUpdater)) as CometUpdater;
        Vector2 dir = c.transform.TransformDirection(new Vector2(-0.5f, -0.5f));
        cometUpdater.direction = dir.normalized;
        cometUpdater.player = player;
    }

    IEnumerator CometWave() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(5f, 10f));
            SpawnComet();
        }
    }
}
