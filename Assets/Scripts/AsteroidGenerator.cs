using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public GameObject[] bigAsteroidPrefabs;
    public GameObject[] mediumAsteroidPrefabs;
    public GameObject[] smallAsteroidPrefabs;
    public GameObject[] carrotPrefabs;
    public DeployComet cometDeployer;
    public DistanceCalculator distanceCalculator;
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
    private ObjectPool carrotPool;
    private InfiniteChunks infiniteChunks;
    private Dictionary<Vector2Int, bool> carrotsPicked;
    
    void Start()
    {
        List<GameObject[]> prefabs = new List<GameObject[]> {
            smallAsteroidPrefabs, mediumAsteroidPrefabs, bigAsteroidPrefabs
        };
        asteroidPool = new ObjectPool(prefabs, poolSizePerAsteroidType);
        carrotPool = new ObjectPool(carrotPrefabs, 10);
        infiniteChunks = new InfiniteChunks(regionSize, seed);
        carrotsPicked = new Dictionary<Vector2Int, bool>();
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
                DrawAsteroids(chunkCoord, chunk);
            }
        }
    }

    private void DrawAsteroids(Vector2Int chunkCoord, InfiniteChunks.Chunk chunk) {
        List<Vector3> points = chunk.GetPoints();
        System.Random prng = new System.Random(seed + chunk.GetCoordPairingNumber());
        int carrotIndex = prng.Next(0, points.Count);
        for (int i = 0; i < points.Count; i++) {
            Vector3 point = points[i];
            int size = (int) point.z;
            int variant = prng.Next(0, asteroidPool.GetVariantCountFor(size));
            GameObject asteroid = asteroidPool.GetFromPool(size, variant);
            if (asteroid != null) {
                asteroid.transform.position = (new Vector2(point.x, point.y) - regionSize/2) + chunk.GetChunkOffset();
                asteroid.SetActive(true);
                if (carrotIndex == i && !carrotsPicked.ContainsKey(chunkCoord)) {
                    DrawCarrot(chunkCoord, chunk, asteroid, size);
                }
                if (simulateAsteroids) {
                    var body = asteroid.GetComponent<Rigidbody2D>();
                    body.bodyType = RigidbodyType2D.Dynamic;
                    body.drag = 0f;
                    body.mass = 40f * size;
                    body.AddForce(new Vector2(Random.value * body.mass/3, Random.value * body.mass/3), ForceMode2D.Impulse);
                }
                chunk.AddObject(asteroid);
            }
        }
        chunk.SetVisible(true);
    }

    private void DrawCarrot(Vector2Int chunkCoord, InfiniteChunks.Chunk chunk, GameObject asteroid, int asteroidSize) {
        GameObject carrot = carrotPool.GetFromPool(0);
        if (carrot == null) return;

        Vector2 dir = Random.insideUnitCircle.normalized * (asteroidSize + 1);
        carrot.transform.position = asteroid.transform.position + asteroid.transform.TransformDirection(dir);

        Vector3 targetRotateDir = asteroid.transform.position - carrot.transform.position;
        float angle = (Mathf.Atan2(targetRotateDir.y, targetRotateDir.x) * Mathf.Rad2Deg) - 90f;
        carrot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        FixedJoint2D joint = carrot.GetComponent<FixedJoint2D>();
        joint.connectedBody = asteroid.GetComponent<Rigidbody2D>();

        Carrot script = carrot.GetComponent<Carrot>();
        script.SetPickupCallback(CarrotPickup, chunkCoord);
        carrot.SetActive(true);
        chunk.AddCarrot(carrot);
    }

    public void CarrotPickup(Vector2Int chunkCoord) {
        if (!carrotsPicked.ContainsKey(chunkCoord)) {
            carrotsPicked.Add(chunkCoord, true);
            InfiniteChunks.Chunk chunk = infiniteChunks.GetChunkAt(chunkCoord);
            if (chunk != null) {
                chunk.PickUpCarrot();
            }
        }
    }

    private void CheckForLevelUp() {
        var travelDistance = distanceCalculator.GetGameModePoints();
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
