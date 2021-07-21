using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightText : MonoBehaviour
{
    Text weightText;
    // Start is called before the first frame update
    void Start()
    {
        weightText = GetComponent<Text>();
        weightText.text = "0.50";
    }

    // Update is called once per frame
    public void TextUpdate(float value)
    {
        weightText.text = value.ToString("0.00");
    }
}
