using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class InfiniteChunks {
    
    private Dictionary<Vector2Int, Chunk> terrainChunkDictionary;
    private Vector2Int regionSize;
    private int seed;
    private int chunksVisibleInViewDst = 1;

    Queue<ChunkThreadInfo<Dictionary<Vector2Int, Chunk>>> chunksThreadQueue = new Queue<ChunkThreadInfo<Dictionary<Vector2Int, Chunk>>>();

    public InfiniteChunks(Vector2Int regionSize, int seed) {
        terrainChunkDictionary = new Dictionary<Vector2Int, Chunk>();
        this.regionSize = regionSize;
        this.seed = seed;
    }

    public Vector2Int GetChunkCoord(Vector2 position) {
        int currentChunkCoordX = Mathf.RoundToInt(position.x / regionSize.x);
		int currentChunkCoordY = Mathf.RoundToInt(position.y / regionSize.y);

        return new Vector2Int(currentChunkCoordX, currentChunkCoordY);
    }

    public void Update() {
        if (chunksThreadQueue.Count > 0) {
			for (int i = 0; i < chunksThreadQueue.Count; i++) {
				ChunkThreadInfo<Dictionary<Vector2Int, Chunk>> threadInfo = chunksThreadQueue.Dequeue();
				threadInfo.callback(threadInfo.parameter);
			}
		}
    }

    public void RequestChunks(Vector2Int position, float[] pointRadiuses, int rejectionSamples, Action<Dictionary<Vector2Int, Chunk>> callback) {
        ThreadStart threadStart = delegate {
			Dictionary<Vector2Int, Chunk> chunks = GenerateChunks(position, pointRadiuses, rejectionSamples);
            lock (chunksThreadQueue) {
                chunksThreadQueue.Enqueue(new ChunkThreadInfo<Dictionary<Vector2Int, Chunk>>(callback, chunks));
            }
		};
		new Thread(threadStart).Start();
    }

    public Dictionary<Vector2Int, Chunk> GenerateChunks(Vector2Int position, float[] pointRadiuses, int rejectionSamples) {
        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) {
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) {
                int coordPairingNumber = MathUtils.SignedCantorPair(position.x + xOffset, position.y + yOffset);
                System.Random prng = new System.Random(seed + coordPairingNumber);

				Vector2Int viewedChunkCoord = new Vector2Int (position.x + xOffset, position.y + yOffset);
                Vector2Int chunkOffset = new Vector2Int((xOffset + position.x) * regionSize.x, (yOffset + position.y) * regionSize.y);
				if (!terrainChunkDictionary.ContainsKey(viewedChunkCoord)) {
                    List<Vector3> points = PoissonDiscSampling.GeneratePoints(pointRadiuses, regionSize, prng, rejectionSamples);
                    terrainChunkDictionary.Add(viewedChunkCoord, new Chunk(points, chunkOffset, coordPairingNumber));
				}
			}
		}
        return terrainChunkDictionary;
    }

    public void HideFarawayChunks(Vector2Int currentChunkCoord) {
        float sqrChunksVisibleInViewDst = 2 * 2;
        List<Vector2Int> hideKeys = new List<Vector2Int>();
        foreach (Vector2Int key in terrainChunkDictionary.Keys) {
            if ((key - currentChunkCoord).sqrMagnitude > sqrChunksVisibleInViewDst) {
                hideKeys.Add(key);
            }
        }

        foreach (Vector2Int key in hideKeys) {
            terrainChunkDictionary[key].Destroy();
            terrainChunkDictionary.Remove(key);
        }
    }

    public Chunk GetChunkAt(Vector2Int coord) {
        if (terrainChunkDictionary.ContainsKey(coord)) {
            return terrainChunkDictionary[coord];
        }
        return null;
    }

    public class Chunk {
        List<Vector3> points;
        List<GameObject> objects;
        GameObject carrot;
        Vector2 chunkOffset;
        int coordPairingNumber;
        public bool isVisible;

        public Chunk(List<Vector3> points, Vector2 chunkOffset, int coordPairingNumber) {
            objects = new List<GameObject>();
            this.points = points;
            this.chunkOffset = chunkOffset;
            this.coordPairingNumber = coordPairingNumber;
            this.isVisible = false;
        }

        public void SetVisible(bool isVisible) {
            this.isVisible = isVisible;
        }

        public bool IsVisible() {
            return isVisible;
        }

        public int GetCoordPairingNumber() {
            return coordPairingNumber;
        }

        public List<Vector3> GetPoints() {
            return points;
        }

        public Vector2 GetChunkOffset() {
            return chunkOffset;
        }

        public void AddObject(GameObject gameObject) {
            this.objects.Add(gameObject);
        }

        public void AddCarrot(GameObject carrot) {
            this.carrot = carrot;
        }

        public void PickUpCarrot() {
            this.carrot.SetActive(false);
        }

        public void Destroy() {
            isVisible = false;
			for (int i = 0; i < objects.Count; i++) {
                objects[i].SetActive(false);
            }
            objects.Clear();

            if (carrot) {
                carrot.SetActive(false);
            }
		}
    }

    struct ChunkThreadInfo<T> {
		public readonly Action<T> callback;
		public readonly T parameter;

		public ChunkThreadInfo (Action<T> callback, T parameter) {
			this.callback = callback;
			this.parameter = parameter;
		}

	}
}