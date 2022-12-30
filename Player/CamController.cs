using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Core;


public class CamController : MonoBehaviour
{
    public Transform player;
    public float accel = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainPlayer").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = Vector3.Lerp(transform.position, player.position, Time.fixedDeltaTime*accel);
        pos.z = -10.0f;
        transform.position = pos;
    }
}
