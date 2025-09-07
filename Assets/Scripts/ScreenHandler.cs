using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Required for using UI components like Image

public class ScriptHandler : MonoBehaviour
{
    private int taskIndex = 1;
    public TMP_Text VHText; // Vision Heavy text (like red, green or ...)
    public GameObject[] vHChoices; // the color planes

    private bool waitingForChoice = false;
    private bool startNewTrialEnabled = true;
    private float vHTimer = 3;
    private float choiceTimer = 0f;
    private List<string> colorTextList = new List<string> { "red", "green", "blue", "yellow"};
    private List<Color> colorColorList = new List<Color> { Color.red, Color.blue, Color.green, Color.yellow, Color.white};


    private int vHColorTextIndex = 0;
    private int vHColorColorIndex = 0;
    private List<int> vHColorTextIndexes  = new List<int>();
    private List<int> vHColorColorIndexes = new List<int>();
    private List<int> vHOperatorResponses = new List<int>();

    void Start()
    {
        ColorVHChoices();
        VisionHeavy();
    }

    void Update()
    {
        switch(taskIndex)
        {
            case (int)TaskIndex.visionHeavy:
                HandleVisionHeavy();
                break;

            default:
                break;
        }
    }

    private enum TaskIndex
    {
        visionHeavy = 1,
        audioHeavy  = 2,
        both        = 3
    }
    private enum VHColorTextIndexEnum
    {
        red = 0,
        green = 1,
        blue = 2,
        yellow = 3,
        
    }
    private enum vHOperatorResponsesEnum
    {
        up = 0,
        right = 1,
        down = 2,
        left = 3,
        notChosen = -1,
        undefined = -2
    }

    private void VisionHeavy()
    {
        waitingForChoice = true;
        choiceTimer = 0f;
        vHColorTextIndexes.Add(Random.Range(0, colorTextList.Count));
        vHColorColorIndexes.Add(Random.Range(0, colorColorList.Count));
        VHText.text  = colorTextList [vHColorTextIndexes[vHColorTextIndexes.Count - 1]];
        VHText.color = colorColorList[vHColorColorIndexes[vHColorColorIndexes.Count - 1]];
    }
    private void HandleVisionHeavy()
    {
        if (!waitingForChoice) return;

        choiceTimer += Time.deltaTime;
        Vector2 thumbStickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        Debug.Log($"thumbStickInput: {thumbStickInput}");
        if (thumbStickInput.magnitude > 0.6f && startNewTrialEnabled)
        { // operator showed their choice (moved the thumbstic)
            waitingForChoice = false;

            startNewTrialEnabled = false;
            StartCoroutine(ReEnableNewTrial());
            GetOperatorResponse(thumbStickInput); // Check if the operator chose the correct choice
            VisionHeavy();
        }
        else if (choiceTimer >= 5f)
        {
            waitingForChoice = false;
            vHOperatorResponses.Add((int)vHOperatorResponsesEnum.notChosen);
            VisionHeavy();
        }
    }

    private void GetOperatorResponse(Vector2 thumbStickInput)
    {
        int operatorResponse;

        if      (thumbStickInput.y > 0.6f)  { vHOperatorResponses.Add((int)vHOperatorResponsesEnum.up);    operatorResponse = (int)VHColorTextIndexEnum.red;   }
        else if (thumbStickInput.x > 0.6f)  { vHOperatorResponses.Add((int)vHOperatorResponsesEnum.right); operatorResponse = (int)VHColorTextIndexEnum.green; }
        else if (thumbStickInput.y < -0.6f) { vHOperatorResponses.Add((int)vHOperatorResponsesEnum.down);  operatorResponse = (int)VHColorTextIndexEnum.blue;  }
        else if (thumbStickInput.x < -0.6f) { vHOperatorResponses.Add((int)vHOperatorResponsesEnum.left);  operatorResponse = (int)VHColorTextIndexEnum.yellow;}
        else operatorResponse = (int)vHOperatorResponsesEnum.undefined;

        if (operatorResponse == vHColorTextIndexes[vHColorTextIndexes.Count - 1]) Debug.Log("Choice Correct");
        else Debug.Log("Choice wrong");
    }

    IEnumerator ReEnableNewTrial()
    {
        yield return new WaitForSeconds(0.5f);
        startNewTrialEnabled = true;

    }
    private void ColorVHChoices()
    {
        vHChoices[0].GetComponent<Renderer>().material.color = Color.red;
        vHChoices[1].GetComponent<Renderer>().material.color = Color.green;
        vHChoices[2].GetComponent<Renderer>().material.color = Color.blue;
        vHChoices[3].GetComponent<Renderer>().material.color = Color.yellow;

    }

}