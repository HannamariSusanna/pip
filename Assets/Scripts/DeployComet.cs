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
    private Dictionary<int, List<GameObject>> cometPool = new Dictionary<int, List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        InitPool();
        StartCoroutine(CometWave());
    }

    private void InitPool() {
        for (int i = 0; i < cometPrefabs.Length; i++) {
            cometPool[i] = new List<GameObject>();
            for (int j = 0; j < poolSizePerCometType; j++) {
                GameObject prefab = cometPrefabs[i];
                prefab.SetActive(false);
                GameObject comet = Instantiate(prefab) as GameObject;
                (comet.GetComponent(typeof(CometUpdater)) as CometUpdater).player = player;
                cometPool[i].Add(comet);
            }
        }
    }

    private GameObject GetFromPool(int cometType) {
        for (int i = 0; i < poolSizePerCometType; i++) {
            GameObject comet = cometPool[cometType][i];
            if (!comet.activeSelf) {
                return comet;
            }
        }
        return null;
    }

    private void SpawnComet() {
        int randomCometIndex = random.Next(0, cometPrefabs.Length);
        var comet = GetFromPool(randomCometIndex);
        if (comet != null) {
            Vector2 dir = new Vector2(screenBounds.x * 1.5f, screenBounds.y * 1f);
            if (randomDir) {
                float angle = Random.value * Mathf.PI * 2;
				dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) + dir;
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
