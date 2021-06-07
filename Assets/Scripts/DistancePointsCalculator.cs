using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePointsCalculator : MonoBehaviour
{
    public Transform player;
    public TMPro.TextMeshProUGUI tm;
    public float maxDistance = 0;
    public Vector3 start;

    void Start() {
        start = new Vector3(player.position.x, player.position.y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        maxDistance = Mathf.Max(maxDistance, Vector3.Distance(start, player.position));
        int points = GetPoints();
        tm.text = points.ToString();
    }

    public int GetPoints() {
        return Mathf.FloorToInt(maxDistance);
    }
}
