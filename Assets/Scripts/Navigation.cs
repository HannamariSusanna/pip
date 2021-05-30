using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{

    public Transform player;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir = target.position - player.position;
        float angle = (Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
