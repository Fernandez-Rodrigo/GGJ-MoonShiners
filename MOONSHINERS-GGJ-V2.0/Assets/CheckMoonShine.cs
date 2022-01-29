using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMoonShine : MonoBehaviour
{

    public DialogueCanvasController dialogue;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CheckDialogue()
    {
        if (Inventory.Instance.hasMoonShine)
        {
            dialogue.ActivateCanvasWithText("Gracias me has curado");
        }
        else
        {
            dialogue.ActivateCanvasWithText("Necesito Moonshine si no morire...");
        }
    }
}
