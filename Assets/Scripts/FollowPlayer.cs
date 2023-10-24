using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    private Transform playerTransform;
    private Vector3 offset = new Vector3(0, -0.3f, 0);

    // Start is called before the first frame update
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = (Vector3.forward * playerTransform.position.z) + offset;
    }
}
