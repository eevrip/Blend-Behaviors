using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.Barracuda; //Important to recognize NNModel Field
using Unity.Barracuda.ONNX;//Important to recognize NNModel Field
public class RollerAgent : Agent
{
    Rigidbody rBody;
    public Behaviours behavior;
    public NNModel seekBehaviorModel01;
    public NNModel seekBehaviorModel02;
    public NNModel seekBehaviorModel03;
    public NNModel fleeBehaviorModel;
    float counter =0f;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
    public Transform Target; //Create an object of Transform that saves the info about the gameObject's position, orientation, scale
    //This saves the info about Target aka cube

    public override void OnEpisodeBegin() 
    { //exists in Agent and we override
       //If the Agent falls out of the plane
        if(this.transform.localPosition.y<0)
        {
            this.rBody.angularVelocity = Vector3.zero; //Initializes to zero the angular velocity
            this.rBody.velocity = Vector3.zero;//Initializes to zero the  velocity
            this.transform.localPosition = new Vector3(0,0.5f,0);//Initializes the position to the center
        }
        //Move the target to a new spot randomly
        Target.localPosition = new Vector3(Random.value * 7 - 3.5f, 0.5f, Random.value * 7 - 3.5f); //floor has limits 4X4 in x and z-axis
        counter = 0f;
    }
    //method that is responsible for the observations that the agent must do. In this case the observed elements-features are the position of the agent and of the target and the velocity of agent.
    //Hence 8 values for the feature vector of the neural network
    public override void CollectObservations(VectorSensor sensor) 
    {   //Target and Agent position
        sensor.AddObservation(Target.localPosition); 
        sensor.AddObservation(this.transform.localPosition);
        //Agent velocity-using component of Rigidbody
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);

    }
    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero; //Initialize the force
        controlSignal.x = actionBuffers.ContinuousActions[0]; //x direction of the force that need to be applied. I believe action[] exists in Agent
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);//Add that force to the rigidbody that is the agent


        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        if (behavior == Behaviours.Seek01)
        {
            SetModel("SeekBehavior01", seekBehaviorModel01, default);
            //SeekBehavior01
            if (distanceToTarget < 1.42f)
            {
                SetReward(1f);
                EndEpisode();
            }
            else if (this.transform.localPosition.y < 0)
            {
                EndEpisode();
            }
            else if (this.transform.localPosition.y < 0)
            {

                EndEpisode();
            }
        }
        else if (behavior == Behaviours.Seek02) {
            //SeekBehavior03
            SetModel("SeekBehavior02", seekBehaviorModel02, default);
            if (distanceToTarget < 1.42f)
            {
                SetReward(0.1f);

            }
            else if (this.transform.localPosition.y < 0)
            {
                SetReward(-1.0f);
                EndEpisode();
            }
        }
        else if (behavior == Behaviours.Seek03) {

            //SeekBehavior04
            SetModel("SeekBehavior03", seekBehaviorModel03, default);
            if (distanceToTarget < 1.42f)
            {
                 SetReward(0.1f); 
                 if(counter <= 100f)
                    counter = counter + 0.1f;
                 else
                    EndEpisode();
            }
            else if (this.transform.localPosition.y < 0)
            {
                SetReward(-1.0f); 
                EndEpisode();
            }
        }
        else {//Avoid -Behaviors.Flee
            SetModel("FleeBehavior", fleeBehaviorModel, default);
            if (distanceToTarget >= 2.5f && this.transform.localPosition.y > 0)
            {
                SetReward(0.1f);
            }
            else if (distanceToTarget < 1.42f || this.transform.localPosition.y < 0) {
                SetReward(-1.0f);
                EndEpisode();
            }

       }
        
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");


    }



}
//What behaviors the agent can have
public enum Behaviours
{
    Seek01,
    Seek02,
    Seek03,
    Flee
}
