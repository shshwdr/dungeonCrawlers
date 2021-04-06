using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{

    public GameObject disableItem;
    public string dialogueVariable;
    public GameObject enableItem;
    private void OnDestroy()
    {
        if(disableItem)
        disableItem.SetActive(false);
        if (enableItem)
        {
            enableItem.SetActive(true);
            enableItem.transform.parent = this.transform.parent;
        }
        DialogueLua.SetVariable(dialogueVariable, true);
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
