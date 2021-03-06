using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendBehaviorWithJumpingSkill : MonoBehaviour
{
    AgentWithJumpingSkill flee;
    AgentWithJumpingSkill seek;
    Rigidbody rb;
    Vector3 force;
    float weightFleeX = 0.5f;
    float weightSeekX = 0.5f;
    float weightFleeZ = 0.5f;
    float weightSeekZ = 0.5f;
    float weightFleeY = 0.5f;
    float weightSeekY = 0.5f;
    float jumpMultiplier;
    float forceMultiplier;
    Vector3 forceFlee;
    Vector3 forceSeek;
    int jumpActionFlee;
    int jumpActionSeek;

    // Start is called before the first frame update
    void Start()
    {
        flee = transform.parent.GetChild(2).gameObject.GetComponent<AgentWithJumpingSkill>();
        seek = transform.parent.GetChild(3).gameObject.GetComponent<AgentWithJumpingSkill>();
        jumpMultiplier = flee.jumpMultiplier;
        forceMultiplier = seek.forceMultiplier;
        rb = GetComponent<Rigidbody>();

    }

    // Move Agent depending on actions received from Agent scripts
    public void MoveAgent()
    {
        forceFlee = flee.force;
        forceSeek = seek.force;
        jumpActionFlee = flee.jumpAction;
        jumpActionSeek = seek.jumpAction;
        //Debug.Log(weightFlee * forceFlee + weightSeek * forceSeek);
        //Add new calculated force considering the given weights
        rb.AddForce(new Vector3(weightFleeX * forceFlee.x + weightSeekX * forceSeek.x, 0f, weightFleeZ * forceFlee.z + weightSeekZ * forceSeek.z)*forceMultiplier );
        if ((jumpActionFlee == 1 && flee.grounded) || (jumpActionSeek == 1 && seek.grounded))
            rb.AddForce(new Vector3(0f,1f,0f)*jumpMultiplier, ForceMode.Impulse);
    }


    public void UpdateWeightX(float wgtSeekX)
    {
        weightSeekX = wgtSeekX;
        weightFleeX = 1f - wgtSeekX;
        
    }
    public void UpdateWeightY(float wgtSeekY)
    {
        weightSeekY = wgtSeekY;
        weightFleeY = 1f - wgtSeekY;
    }
    public void UpdateWeighZ(float wgtSeekZ)
    {
        weightSeekZ = wgtSeekZ;
        weightFleeZ = 1f - wgtSeekZ;
        
    }
}

