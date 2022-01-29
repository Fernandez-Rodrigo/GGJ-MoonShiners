using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Canvas missionCanvas;
    public Canvas dialogueCanvas;

    public void ShowMission()
    {
        missionCanvas.gameObject.SetActive(true);
    }

    public void HideMission()
    {
        missionCanvas.gameObject.SetActive(false);
    }


    public void ShowDialogue()
    {
        dialogueCanvas.gameObject.SetActive(true);
    }

    public void HideDialogue()
    {
        dialogueCanvas.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
             ShowMission();
            GameManager.GameManagerInstance.PauseGame();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            HideMission();
            GameManager.GameManagerInstance.ContinueGame();
        }
    }

}
