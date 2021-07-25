# Blend-Behaviors
An effort to blend two simple behaviors that are trained using RL.

First Step: Train Flee and Seek Behavior using ML-Agents.

-Find RollerAgent.cs by going to Assets>Scripts>Scene_WeightOverCrowd>RollerAgent.cs. This script was used to trained the desired behaviors. The goal is to not fall down the platform and trying to continuously avoid or reach the target. For the seek behavior, three different approaches were used, hence there are three models named SeekBehavior01, SeekBehavior03, and SeekBehavior04 inside Assets>TFModels folder. For the flee behavior only one approach was used, and the exported model is named as FleeBehavior2.

Second Step: Blend these two simple behaviors.

Go to scene "A-Blending Behaviors(Flee-Seek)"

-After training we have models that can be set as a brain for the agent. We try to blend the outputs and not the weights of the model. The outputs can be aquired using OnActionReceived() method that exist in Agent class (see ML-agents documentation for more information). We can add force in the desired directions only inside this method. 
-How to blend: we need to get at a specific time frame the outputs (actions) from both models. The issue here is that the Agent class has only one brain-You can't have two model or more simultaneously. The actions that are received in the function OnActionReceived() come from only one model at a time. To solve this, I created two empty object as children of TrainingArea prefab, that have a script inheriting from the class Agent. The objects are called FleeAgent and SeekAgent, and the script attached to them are called AgentMovement.cs. The script has its own model depending on the name of the gameobject that is attached to it. The model can be added in the editor by the user. This script is not attached to the agent itself but it gets the required information from the agent and receive the appropriate actions. To add force to the agent, we get the actions from AgentMovement.cs that is attached to each empty gameobjects inside the script BlendBehavior.cs that is attached to the agent. This is done calling the method MoveAgent(). The method is called inside the OnActionReceived() and only inside FleeAgent.cs. (This might get chagned.-Needs discussion)
-MoveAgent(), gets the output from both models and blends the actions by using weights over each action.

Scene:B-Blending Behaviors(Flee-Seek)-Jumping Skill
This scene allows the agent to jump. The appropriate models were received while using the scene TrainingWithJump.

Note: There is another scene called "Weight ocer Crowd", that changes the model of each agent according to the users input. Eg. if 0.4 is given as an input, from the 12 agents in the scene the 5 of them behave according to the Seek behavior and the rest of them with the Flee Behavior.
