using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendBehavior : MonoBehaviour
{
    FleeAgent flee;
    SeekAgent seek;
    Rigidbody rb;
    Vector3 force;
    float weightFlee =0.5f;
    float weightSeek = 0.5f;
    public float forceMultiplier = 10f;
    Vector3 forceFlee;
    Vector3 forceSeek;

    // Start is called before the first frame update
    void Start()
    {
        flee = transform.parent.GetChild(2).gameObject.GetComponent<FleeAgent>();
        seek = transform.parent.GetChild(3).gameObject.GetComponent<SeekAgent>();
        rb = GetComponent<Rigidbody>();
        
    }

    // Move Agent depending on actions received from Agent scripts
    public void MoveAgent()
    {
        forceFlee = flee.forceFlee;
        forceSeek = seek.forceSeek;
        Debug.Log(weightFlee*forceFlee + weightSeek*forceSeek);
        //Add new calculated force considering the given weights
        rb.AddForce((weightFlee* forceFlee + weightSeek * forceSeek) * forceMultiplier);
    }

    public void UpdateWeight(float wgtSeek) {
        
        weightSeek = wgtSeek;
        weightFlee = 1f - wgtSeek;
    }
}
