using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCalculator : MonoBehaviour, PointsCalculator
{
    public Transform player;
    public TMPro.TextMeshProUGUI tm;
    private float maxDistance = 0;
    private Vector3 start;

    void Start() {
        start = new Vector3(player.position.x, player.position.y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        maxDistance = Mathf.Max(maxDistance, Vector3.Distance(start, player.position));
        int points = GetGameModePoints();
        tm.text = points.ToString();
    }

    public int GetGameModePoints() {
        return Mathf.FloorToInt(maxDistance);
    }
}
