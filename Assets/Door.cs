using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        if (isInteracting)
        {
            return;
        }
        isInteracting = true;
        DialogueManager.ShowAlert("Kill Guardian to pass", showTime);
    }
}
