# Blend-Behaviors
An effort to blend two simple behaviors that are trained using RL.

First Step: Train Flee and Seek Behavior using ML-Agents.

-Find RollerAgent.cs by going to Assets>Scripts>Scene_WeightOverCrowd>RollerAgent.cs. This script was used to trained the desired behaviors. The goal is to not fall down the platform and trying to continuously avoid or reach the target. For the seek behavior, three different approaches were used, hence there are three models named SeekBehavior01, SeekBehavior03, and SeekBehavior04 inside Assets>TFModels folder. For the flee behavior only one approach was used, and the exported model is named as FleeBehavior2.

Second Step: Blend these two simple behaviors.

-After training we have models that can be set as a brain for the agent. We try to blend the outputs and not the weights of the model. The outputs can be aquired using OnActionReceived() method that exist in Agent class (see ML-agents documentation for more information). We can add force in the desired directions only inside this method. 
-How to blend: we need to get at a specific time frame the outputs (actions) from both models. The issue here is that the Agent class has only one brain-You can't have two model or more simultaneously. The actions that are received in the function OnActionReceived() come from only one model at a time. To solve this, I created two empty object as children of TrainingArea prefab, that have a script inheriting from the class Agent. The objects are called FleeBehavior and SeekBehavior, and the scripts attached to them are called FleeAgent.cs and SeekAgent.cs respectivelly. Each script has its own model, that can be added in the editor by the user. The difference between these scripts and RollerAgent.cs is that they are not attached to the agent itself. They get the required information from the agent and receive the appropriate actions. To add force to the agent, we get the actions from these two scripts inside the script BlendBehavior.cs that is attached to the agent. This is done calling the method MoveAgent(). The method is called inside the OnActionReceived() and only inside FleeAgent.cs. (This might get chagned.-Needs discussion)
-MoveAgent(), gets the output from both models and blends the actions by using weights over each action.


