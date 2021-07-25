using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendBehavior : MonoBehaviour
{
    AgentMovement flee;
   AgentMovement seek;
    Rigidbody rb;
    Vector3 force;
    float weightFleeX = 0.5f;
    float weightSeekX = 0.5f;
    float weightFleeZ = 0.5f;
    float weightSeekZ = 0.5f;

    public float forceMultiplier = 10f;
    Vector3 forceFlee;
    Vector3 forceSeek;

    // Start is called before the first frame update
    void Start()
    {
        flee = transform.parent.GetChild(2).gameObject.GetComponent<AgentMovement>();
        seek = transform.parent.GetChild(3).gameObject.GetComponent<AgentMovement>();
        rb = GetComponent<Rigidbody>();
        
    }

    // Move Agent depending on actions received from Agent scripts
    public void MoveAgent()
    {
        forceFlee = flee.force;
        forceSeek = seek.force;
        Debug.Log(weightFleeX*forceFlee + weightSeekX*forceSeek);
        //Add new calculated force considering the given weights
        rb.AddForce(new Vector3(weightFleeX * forceFlee.x + weightSeekX * forceSeek.x, 0f , weightFleeZ * forceFlee.z + weightSeekZ * forceSeek.z) * forceMultiplier);
    }

    public void UpdateWeightX(float wgtSeekX)
    {
        weightSeekX = wgtSeekX;
        weightFleeX = 1f - wgtSeekX;

    }
   
    public void UpdateWeighZ(float wgtSeekZ)
    {
        weightSeekZ = wgtSeekZ;
        weightFleeZ = 1f - wgtSeekZ;

    }
}
