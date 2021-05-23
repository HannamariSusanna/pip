using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustDust : MonoBehaviour
{

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + player.TransformDirection(new Vector2(15, 0));
        transform.eulerAngles = player.eulerAngles;
    }
}
