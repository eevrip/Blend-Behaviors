using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.Barracuda; //Important to recognize NNModel Field
using Unity.Barracuda.ONNX;//Important to recognize NNModel Field
public class AgentWithJumpingSkill : Agent
{ 
    Rigidbody rBody;
    Transform tBody;
    BlendBehaviorWithJumpingSkill blend;
    public NNModel fleeModel;
    public NNModel seekModel;
    public Vector3 force;
    public int jumpAction;
    public float forceMultiplier = 15f;
    public float jumpMultiplier = 7f;
    public Transform Target;
    public LayerMask groundLayers;
    public bool grounded;
    string nameAgentType;

    public override void Initialize()
    {
        tBody = transform.parent.GetChild(4);
        rBody = tBody.gameObject.GetComponent<Rigidbody>();
        nameAgentType = this.name;
       blend = tBody.gameObject.GetComponent<BlendBehaviorWithJumpingSkill>();

    }

    bool isGrounded() {        
        return Physics.CheckSphere(tBody.position , 0.6f, groundLayers, QueryTriggerInteraction.Ignore);
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
        Target.localPosition = new Vector3(Random.value * 7 - 3.5f, 0.5f, Random.value * 7 - 3.5f); //floor has limits 8 units in x and z-axis

    }

    //Method that is responsible for what the agents observes. In this case the observed elements-features are the position of the agent, the target and the velocity of agent.
    //Hence 9 values for the feature vector of the neural network
    public override void CollectObservations(VectorSensor sensor)
    {   //Target and Agent position
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(tBody.localPosition);
        //Agent velocity-using component of Rigidbody
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.y);
        sensor.AddObservation(rBody.velocity.z);

    }

    //Actions to take that are best due to current observations
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
       
         //Set Model to existing model of choice in Editor. This is used when no trainning and ignored when trainning
        //Note: for details of how the models were trainned, look at the script RollerAgent, which is inside the folder: Assets>Scripts>Scene_WeightOverCrowd

        force.x = actionBuffers.ContinuousActions[0]; //x direction of force that needs to be applied. 
        force.z = actionBuffers.ContinuousActions[1];//y direction
        jumpAction = actionBuffers.DiscreteActions[0];//jump

        float distanceToTarget = Vector3.Distance(tBody.localPosition, Target.localPosition);

        if(nameAgentType == "FleeAgent")
        {
            SetModel("FleeBehaviorWithJumpingSkill", fleeModel, default);
            //Note: The follow ccode was used for training. This section is se to a comment when blending
           /* if (distanceToTarget >= 2.5f && tBody.localPosition.y > 0)
            {
                SetReward(0.1f);
            }
            else if (distanceToTarget < 1.42f ||tBody.localPosition.y < 0)
            {
                SetReward(-1.0f);
                EndEpisode();
            }
           */
            grounded = isGrounded();
            blend.MoveAgent();//Call the MoveAgent() function to blend the flee force and seek force
        }
        else if (nameAgentType == "SeekAgent")
        {
            SetModel("SeekBehaviorWithJumpingSkill", seekModel, default);
            //Note: The follow ccode was used for training. This section is se to a comment when blending
            /*if (distanceToTarget < 1.42f)
            {
                SetReward(0.1f);

            }
            else if (tBody.localPosition.y < 0)
            {
                SetReward(-1.0f);
                EndEpisode();
            }*/
            grounded = isGrounded();
        }

        //The follow two lines are used when blending the behaviors. They have to set to a comment when training
        if (tBody.localPosition.y < 0)
            EndEpisode();

        //The follow section of code is used when training. This is set to a comment when blending.
        //  rBody.AddForce(force* forceMultiplier);//Add that force to the rigidbody that is the agent
        //  grounded = isGrounded();
        // if (jumpAction == 1 && isGrounded())
        //    rBody.AddForce(new Vector3(0f,1f,0f)*jumpMultiplier, ForceMode.Impulse);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut.Clear();
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
        grounded = isGrounded();
        if (Input.GetKey("space") && isGrounded())
            discreteActionsOut[0] = 1;
        


    }



}