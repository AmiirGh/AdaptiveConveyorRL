using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    void Start()
    {
        ColorVisionHeavyChoices();
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


    private void VisionHeavy()
    {
        waitingForChoice = true;
        choiceTimer = 0f;
        string text = colorTextList[Random.Range(0, colorTextList.Count)];
        VHText.text  = colorTextList [Random.Range(0, colorTextList.Count)];
        VHText.color = colorColorList[Random.Range(0, colorTextList.Count)];
    }
    private void HandleVisionHeavy()
    {
        if (!waitingForChoice) return;
        choiceTimer += Time.deltaTime;
        Vector2 rightStick = Vector2.zero;
        Vector2 thumbStickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        Debug.Log($"thumbStickInput: {thumbStickInput}");
        if (thumbStickInput.x > 0.5f && startNewTrialEnabled)
        {
            startNewTrialEnabled = false;
            StartCoroutine(ReEnableNewTrial());
            waitingForChoice = false;
            VisionHeavy();
        }
        else if (choiceTimer >= 5f)
        {
            waitingForChoice = false;
            VisionHeavy();
        }
    }

    IEnumerator ReEnableNewTrial()
    {
        yield return new WaitForSeconds(0.5f);
        startNewTrialEnabled = true;

    }
    private void ColorVisionHeavyChoices()
    {
        vHChoices[0].GetComponent<Renderer>().material.color = Color.red;
        vHChoices[1].GetComponent<Renderer>().material.color = Color.green;
        vHChoices[2].GetComponent<Renderer>().material.color = Color.blue;
        vHChoices[3].GetComponent<Renderer>().material.color = Color.yellow;

    }

}