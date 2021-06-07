using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public GameObject[] bigAsteroidPrefabs;
    public GameObject[] mediumAsteroidPrefabs;
    public GameObject[] smallAsteroidPrefabs;
    public DeployComet cometDeployer;
    public DistancePointsCalculator pointsCalculator;
    public TMPro.TextMeshProUGUI levelText;
  
    public Transform player;
    Vector3 playerPositionOld = Vector2.zero;
    const float sqrDistanceToTravelForUpdate = 10 * 10;
    
    public float[] radiuses = new float[] { 2, 3, 5 };
    public Vector2Int regionSize = Vector2Int.one;
    public int rejectionSamples = 10;

    public int poolSizePerAsteroidType = 20;
    public static float maxViewDst = 25f;
    public int seed = 2002;
    public int level = 1;
    public bool simulateAsteroids = false;
    
    private ObjectPool asteroidPool;
    private InfiniteChunks infiniteChunks;
    
    void Start()
    {
        List<GameObject[]> prefabs = new List<GameObject[]> {
            smallAsteroidPrefabs, mediumAsteroidPrefabs, bigAsteroidPrefabs
        };
        asteroidPool = new ObjectPool(prefabs, poolSizePerAsteroidType);
        infiniteChunks = new InfiniteChunks(regionSize, seed);
        GenerateLevel();
    }

    void Update() {
        CheckForLevelUp();
        infiniteChunks.Update();
        if ((playerPositionOld - player.position).sqrMagnitude > sqrDistanceToTravelForUpdate) {
            playerPositionOld = new Vector2(player.position.x, player.position.y);
            GenerateLevel();
        }
    }

    void OnValidate() {
        if (regionSize.x <= 0) {
            regionSize.x = 1;
        }
        if (regionSize.y <= 0) {
            regionSize.y = 1;
        }
    }

    private void GenerateLevel() {
        Vector2Int currentChunkCoord = infiniteChunks.GetChunkCoord(player.position);
        infiniteChunks.HideFarawayChunks(currentChunkCoord);
        infiniteChunks.RequestChunks(currentChunkCoord, radiuses, rejectionSamples, OnChunksReceived);
    }

    void OnChunksReceived(Dictionary<Vector2Int, InfiniteChunks.Chunk> chunks) {
        foreach (Vector2Int chunkCoord in chunks.Keys) {
            InfiniteChunks.Chunk chunk = chunks[chunkCoord];
            if (!chunk.IsVisible()) {
                DrawAsteroids(chunk);
            }
        }
    }

    private void DrawAsteroids(InfiniteChunks.Chunk chunk) {
        List<Vector3> points = chunk.GetPoints();
        for (int i = 0; i < points.Count; i++) {
            Vector3 point = points[i];
            int size = (int) point.z;
            System.Random prng = new System.Random(seed + chunk.GetCoordPairingNumber());
            int variant = prng.Next(0, asteroidPool.GetVariantCountFor(size));
            GameObject asteroid = asteroidPool.GetFromPool(size, variant);
            if (asteroid != null) {
                asteroid.transform.position = (new Vector2(point.x, point.y) - regionSize/2) + chunk.GetChunkOffset();
                asteroid.SetActive(true);
                if (simulateAsteroids) {
                    var body = asteroid.GetComponent<Rigidbody2D>();
                    body.bodyType = RigidbodyType2D.Dynamic;
                    body.drag = 0f;
                    body.mass = 40f * size;
                    body.AddForce(new Vector2(Random.value * body.mass/2, Random.value * body.mass/2), ForceMode2D.Impulse);
                }
                chunk.AddObject(asteroid);
            }
        }
        chunk.SetVisible(true);
    }

    private void CheckForLevelUp() {
        var travelDistance = pointsCalculator.maxDistance;
        var nextLevel = Mathf.RoundToInt(travelDistance / (40 * Mathf.Log10(travelDistance + 2))) + 1;
        if (nextLevel != level) {
            levelText.text = "Level " + nextLevel;
            level = nextLevel;
            if (level >= 2) {
                simulateAsteroids = true;
            }
            if (level >= 3) {
                cometDeployer.randomDir = true;
                cometDeployer.deployInterval = new Vector2(5, 7);
            }
            if (level >= 4) {
                rejectionSamples += 10;
                radiuses[radiuses.Length-1] -= 1;
            }
            if (level >= 5) {
                radiuses[Mathf.FloorToInt(radiuses.Length/2)] -= 1;
            }
        }
    }
}
