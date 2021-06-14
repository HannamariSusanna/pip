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
        float playerDistance = Vector3.Distance(start, player.position);
        maxDistance = Mathf.Max(maxDistance, playerDistance);
        int points = GetGameModePoints();
        tm.text = points.ToString();
        if (playerDistance < maxDistance) {
            Color color = Mathf.RoundToInt(Time.time) % 2 == 0 ? new Color(1, 0, 0, 1) : new Color(1, 1, 1, 1);
            tm.color = color;
        } else {
            tm.color = new Color(1, 1, 1, 1);
        }
    }

    public int GetGameModePoints() {
        return Mathf.FloorToInt(maxDistance);
    }
}
