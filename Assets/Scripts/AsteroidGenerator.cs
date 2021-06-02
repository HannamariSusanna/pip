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
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 10;

    public int poolSizePerAsteroidType = 20;
    public static float maxViewDst = 25f;
    public int seed = 2002;
    public int level = 1;
    public bool simulateAsteroids = false;

    private enum AsteroidSize { small = 0, medium = 1, big = 2 }
 

    private Dictionary<int, List<GameObject>> bigAsteroidPool = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, List<GameObject>> mediumAsteroidPool = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, List<GameObject>> smallAsteroidPool = new Dictionary<int, List<GameObject>>();
    
    Dictionary<Vector2, AsteroidChunk> terrainChunkDictionary = new Dictionary<Vector2, AsteroidChunk>();
    int chunksVisibleInViewDst = 1;
    
    void Start()
    {
        InitPool(bigAsteroidPrefabs, bigAsteroidPool);
        InitPool(mediumAsteroidPrefabs, mediumAsteroidPool);
        InitPool(smallAsteroidPrefabs, smallAsteroidPool);

        GenerateAsteroidChunks();
    }

    void Update() {
        CheckForLevelUp();
        if ((playerPositionOld - player.position).sqrMagnitude > sqrDistanceToTravelForUpdate) {
            playerPositionOld = new Vector2(player.position.x, player.position.y);
            GenerateAsteroidChunks();
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

    private void CheckForLevelUp() {
        var travelDistance = pointsCalculator.maxDistance;
        var nextLevel = Mathf.RoundToInt(travelDistance / (40 * Mathf.Log(travelDistance + 2))) + 1;
        if (nextLevel != level) {
            levelText.text = "Level " + nextLevel;
            level = nextLevel;
            if (level >= 1) {
                
            }
            if (level >= 2) {
                cometDeployer.randomDir = true;
            }
            if (level >= 3) {
                cometDeployer.deployInterval = new Vector2(5, 7);
            }
            if (level >= 4) {
                rejectionSamples += 10;
                radiuses[radiuses.Length-1] -= 1;
            }
            if (level >= 5) {
                radiuses[Mathf.FloorToInt(radiuses.Length/2)] -= 1;
                simulateAsteroids = true;
            }
        }
    }

    private void GenerateAsteroidChunks() {
        int currentChunkCoordX = Mathf.RoundToInt(player.position.x / regionSize.x);
		int currentChunkCoordY = Mathf.RoundToInt(player.position.y / regionSize.y);

        HideChunks(new Vector2(currentChunkCoordX, currentChunkCoordY));

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) {
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) {
                int coordPairingNumber = MathUtils.SignedCantorPair(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                System.Random prng = new System.Random(seed + coordPairingNumber);

				Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                Vector2 chunkOffset = new Vector2((xOffset + currentChunkCoordX) * regionSize.x, (yOffset + currentChunkCoordY) * regionSize.y);
				if (!terrainChunkDictionary.ContainsKey(viewedChunkCoord)) {
                    List<Vector3> points = PoissonDiscSampling.GeneratePoints(radiuses, regionSize, prng, rejectionSamples);
                    List<GameObject> asteroids = GetAsteroids(points, prng, chunkOffset, viewedChunkCoord);
                    terrainChunkDictionary.Add(viewedChunkCoord, new AsteroidChunk(asteroids));
				}
			}
		}
    }

    private List<GameObject> GetAsteroids(List<Vector3> points, System.Random prng, Vector2 chunkOffset, Vector2 chunkCoord) {
        List<GameObject> asteroids = new List<GameObject>();
        for (int i = 0; i < points.Count; i++) {
            Vector3 point = points[i];
            AsteroidSize size = (AsteroidSize)((int)point.z);
            GameObject asteroid = GetFromPool(size, prng);
            if (asteroid != null) {
                asteroid.transform.position = (new Vector2(point.x, point.y) - regionSize/2) + chunkOffset;
                asteroid.SetActive(true);
                if (simulateAsteroids) {
                    var body = asteroid.GetComponent<Rigidbody2D>();
                    body.bodyType = RigidbodyType2D.Dynamic;
                    body.drag = 0f;
                    body.mass = 100f;
                    body.AddForce(new Vector2(Random.value * 20, Random.value * 20), ForceMode2D.Impulse);
                }
                asteroids.Add(asteroid);
            }
        }
        return asteroids;
    }

    private void HideChunks(Vector2 currentChunkCoord) {
        float sqrChunksVisibleInViewDst = 2 * 2;
        List<Vector2> hideKeys = new List<Vector2>();
        foreach (Vector2 key in terrainChunkDictionary.Keys) {
            if ((key - currentChunkCoord).sqrMagnitude > sqrChunksVisibleInViewDst) {
                hideKeys.Add(key);
            }
        }

        foreach (Vector2 key in hideKeys) {
            terrainChunkDictionary[key].Destroy();
            terrainChunkDictionary.Remove(key);
        }
    }

    private void InitPool(GameObject[] prefabs, Dictionary<int, List<GameObject>> pool) {
        for (int i = 0; i < prefabs.Length; i++) {
            pool[i] = new List<GameObject>();
            for (int j = 0; j < poolSizePerAsteroidType; j++) {
                GameObject asteroid = Instantiate(prefabs[i]) as GameObject;
                asteroid.SetActive(false);
                pool[i].Add(asteroid);
            }
        }
    }

    private GameObject GetFromPool(AsteroidSize size, System.Random prng) {
        Dictionary<int, List<GameObject>> pool =
            size == AsteroidSize.big ? bigAsteroidPool :
            size == AsteroidSize.medium ? mediumAsteroidPool :
            smallAsteroidPool;

        if (pool.Count > 0) {
            int type = prng.Next(0, pool.Count);
            
            for (int i = 0; i < poolSizePerAsteroidType; i++) {
                GameObject asteroid = pool[type][i];
                if (!asteroid.activeSelf) {
                    return asteroid;
                }
            }
        }
        return null;
    }

    public class AsteroidChunk {

		List<GameObject> asteroids;

        public AsteroidChunk(List<GameObject> asteroids) {
            this.asteroids = asteroids;
        }

        public void Update() {
            /*for (int i = 0; i < asteroids.Count; i++) {
                asteroids[i].GetComponent<Rigidbody2D>().velocity = 
            }*/
        }

        public void Destroy() {
			for (int i = 0; i < asteroids.Count; i++) {
                asteroids[i].SetActive(false);
            }
            asteroids.Clear();
		}
    }
}
