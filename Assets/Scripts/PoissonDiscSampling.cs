
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PoissonDiscSampling {
    public static List<Vector3> GeneratePoints(float[] radiuses, Vector2 sampleRegionSize, System.Random prng, int numSamplesBeforeRejection) {
        float cellSize = radiuses[0]/Mathf.Sqrt(2);
        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x/cellSize), Mathf.CeilToInt(sampleRegionSize.y/cellSize)];
        List<Vector3> points = new List<Vector3>(); // Store size index in z
        List<Vector3> spawnPoints = new List<Vector3>();

        spawnPoints.Add(new Vector3(sampleRegionSize.x/2, sampleRegionSize.y/2 + 2, 0)); // +2y from the center of the region
        int iterations = 0;
        int maxIterations = 500;
        while(spawnPoints.Count > 0) {
            iterations++;
            if (iterations >= maxIterations) return points;

            int spawnIndex = prng.Next(spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++) {
                float angle = ((float)prng.NextDouble()) * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                int randomRadiusIndex = prng.Next(0, radiuses.Length);
                Vector2 location = spawnCentre + dir * 2 * radiuses[randomRadiusIndex];
                Vector3 candidate = new Vector3(location.x, location.y, randomRadiusIndex);

                if (IsValid(candidate, sampleRegionSize, cellSize, radiuses, points, grid)) {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)((candidate.x)/cellSize), (int)((candidate.y)/cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }

            if (!candidateAccepted) {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return points;
    }
    static bool IsValid(Vector3 candidate, Vector2 sampleRegionSize, float cellSize, float[] radiuses, List<Vector3> points, int[,] grid) {
        float radius = candidate.z/2;
        if (candidate.x - radius >= 0 && candidate.x + radius < sampleRegionSize.x && candidate.y - radius >= 0 && candidate.y + radius < sampleRegionSize.y) {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0)-1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1)-1);

            for (int x = searchStartX; x <= searchEndX; x++) {
                for (int y = searchStartY; y <= searchEndY; y++) {
                    int pointIndex = grid[x,y]-1;
                    if (pointIndex != -1) {
                        float dst = Vector2.Distance(candidate, points[pointIndex]) - radiuses[(int)candidate.z] - radiuses[(int)points[pointIndex].z];
                        if (dst < 0) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }
}