using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityMenu : Singleton<AbilityMenu>
{
    [SerializeField]
    Transform buttonsParent;
    List<AbilityMenuButton> abilityButtons = new List<AbilityMenuButton>();
    public TMP_Text detailLabel;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform buffTransform in buttonsParent)
        {
            abilityButtons.Add(buffTransform.GetComponent<AbilityMenuButton>());
        }
        showMenu();
    }

    public void showMenu()
    {
        if (abilityButtons.Count == 0)
        {
            return;
        }
        int i = 0;
        foreach (var actionInfo in AbilityManager.Instance.abilityDict.Values)
        {
            if (actionInfo.actionId == "Attack")
            {
                continue;
            }
            if (actionInfo.isUnlocked)
            {

                // GameObject button = Instantiate(buttonPrefab, buttonsParent);
                //ActionButton actionButton = button.GetComponent<ActionButton>();
                abilityButtons[i].Init(actionInfo);
                abilityButtons[i].gameObject.SetActive(true);
                //abilityButtons[actionInfo.actionId] = actionButton;
                //button.SetActive(false);
                //abilityLevel[actionInfo.actionId] = 0;
                //abilityExp[actionInfo.actionId] = 0;
                i++;
            }
        }
        for(;i< abilityButtons.Count; i++)
        {
            abilityButtons[i].gameObject.SetActive(false);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
