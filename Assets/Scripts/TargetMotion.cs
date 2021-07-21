using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMotion : MonoBehaviour
{
    public GameObject point;
    public float speed = 20f;
    // Update is called once per frame
    
    void Update()
    {
        transform.RotateAround(point.transform.position, Vector3.up, Time.deltaTime * speed);
    }
}
