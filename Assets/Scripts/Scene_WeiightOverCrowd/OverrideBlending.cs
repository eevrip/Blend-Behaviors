using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverrideBlending : MonoBehaviour
{

    public GameObject[] agents; //array of agents
    RollerAgent[] brain; //array of brain
    int countSeek;


    void Awake() 
    {  

        agents = GameObject.FindGameObjectsWithTag("Agent"); //finds all the agents in the scene 
    }
    // Start is called before the first frame update
    void Start()
    {
      //Randomize order of the array
      for(int i = 0; i < agents.Length; i++)
        {
            GameObject temp = agents[i];
            int randomNum = Random.Range(0, agents.Length);
            agents[i] = agents[randomNum];
            agents[randomNum] = temp;
        }
       
      Debug.Log("Number of agents " + agents.Length);
      if (agents.Length > 0)
      {
          brain = new RollerAgent[agents.Length];
          for (int i = 0; i < agents.Length; i++)
          {
              if(agents[i] != null)
                  brain[i] = agents[i].GetComponent<RollerAgent>();
          }
      }
        
      WeightUpdate(0.5f); //Initialize to half and half
    }

    // Weight update using slider UI
    public void WeightUpdate(float weightSeek)
    {
        
        countSeek = Mathf.RoundToInt(weightSeek * agents.Length); //round the result.
        Debug.Log("There are " + agents.Length + " agents. The weight of Seek behavior is " + weightSeek + ". The count is " + countSeek);
        for (int i = 0; i < countSeek; i++) {
            brain[i].behavior = Behaviours.Seek01;
        }
        for(int i = countSeek; i < agents.Length; i++)
        {
            brain[i].behavior = Behaviours.Flee;
        }

    }
}
