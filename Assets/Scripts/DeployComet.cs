using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployComet : MonoBehaviour
{
    public GameObject[] cometPrefabs;
    public Transform player;
    public int poolSizePerCometType = 5;
    public bool randomDir = false;
    public Vector2 deployInterval = new Vector2(5, 10);
    private Vector2 screenBounds;
    private System.Random random;
    private ObjectPool cometPool;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        cometPool = new ObjectPool(cometPrefabs, poolSizePerCometType, transform.parent);
        StartCoroutine(CometWave());
    }

    private void SpawnComet() {
        int variant = random.Next(0, cometPrefabs.Length);
        var comet = cometPool.GetFromPool(variant);
        comet.GetComponent<CometUpdater>().player = player;
        if (comet != null) {
            Vector2 dir = new Vector2(screenBounds.x * 1.5f, screenBounds.y * 1f);
            if (randomDir) {
				dir = Random.insideUnitCircle.normalized * (screenBounds.x + 1);
            }
            comet.transform.position = player.position + player.TransformDirection(dir);
            comet.transform.eulerAngles = player.eulerAngles;
            comet.SetActive(true);
        }
    }

    IEnumerator CometWave() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(deployInterval.x, deployInterval.y));
            SpawnComet();
        }
    }
}
