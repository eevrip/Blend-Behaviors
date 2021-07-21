using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.Barracuda; //Important to recognize NNModel Field
using Unity.Barracuda.ONNX;//Important to recognize NNModel Field
public class SeekAgent : Agent
{
    Rigidbody rBody;
    Transform tBody;
    BlendBehavior blend;
    public NNModel seekModel;
    public Vector3 forceSeek;
    public float forceMultiplier = 10;
    public Transform Target; 

    public override void Initialize()
    {
        tBody = transform.parent.GetChild(4);
        rBody = tBody.gameObject.GetComponent<Rigidbody>();
        blend = tBody.gameObject.GetComponent<BlendBehavior>();

    }


    public override void OnEpisodeBegin()
    { 
      //If the Agent falls out of the plane
        if (tBody.localPosition.y < 0)
        {
            rBody.angularVelocity = Vector3.zero; //Initializes to zero the angular velocity
            rBody.velocity = Vector3.zero;//Initializes to zero the  velocity
            tBody.localPosition = new Vector3(0, 0.5f, 0);//Initializes the position to the center
        }
        //Move the target to a new spot randomly
        Target.localPosition = new Vector3(Random.value * 7 - 3.5f, 0.5f, Random.value * 7 - 3.5f); //floor has limits 4X4 in x and z-axis

    }

    //Method that is responsible for what the agents observes. In this case the observed elements-features are the position of the agent, the target and the velocity of agent.
    //Hence 8 values for the feature vector of the neural network
    public override void CollectObservations(VectorSensor sensor)
    {   //Target and Agent position
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(tBody.localPosition);
        //Agent velocity-using component of Rigidbody
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);

    }

    //Actions to take that are best due to current observations
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        SetModel("SeekBehavior", seekModel, default);//Set Model to existing model of choice in Editor. This is used when no trainning and ignored when trainning
        //Note: for details of how the models were trainned, look at the script RollerAgent, which is inside the folder: Assets>Scripts>Scene_WeightOverCrowd

        forceSeek.x = actionBuffers.ContinuousActions[0]; //x direction of the force that need to be applied. 
        forceSeek.z = actionBuffers.ContinuousActions[1];//z direction
        float distanceToTarget = Vector3.Distance(tBody.localPosition, Target.localPosition);

        if (distanceToTarget < 1.42f || tBody.localPosition.y < 0)
        {

            EndEpisode();
        }

        blend.MoveAgent();
        //rBody.AddForce(forceSeek * forceMultiplier);//Add that force to the rigidbody that is the agent

    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");


    }



}
