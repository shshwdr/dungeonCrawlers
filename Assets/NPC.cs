using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public string dialog_title;
    // Start is called before the first frame update
    void Start()
    {

    }


    public override void Interact()
    {
        if (isInteracting)
        {
            return;
        }
        isInteracting = true;
        DialogueManager.StartConversation(dialog_title);
        //DialogueManager.ShowAlert("This is a frog", showTime);
    }
}
