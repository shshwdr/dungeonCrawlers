using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSDialogueManager : Singleton<CSDialogueManager>
{
    public bool isInDialogue;

    public void getInDialog()
    {
        isInDialogue = true;
    }
    public void exitDialog()
    {
        isInDialogue = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
