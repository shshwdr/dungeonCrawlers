using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuButton : MonoBehaviour
{
    AbilityInfo item;
    public TMP_Text nameLabel;
    public TMP_Text levelLabel;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        if (item != null)
        {
            updateUI();
        }
        button = GetComponent<Button>();
    }

    void updateUI()
    {
            nameLabel.text = item.actionName;
            levelLabel.text = "LVL "+AbilityManager.Instance.abilityLevel[item.actionId].ToString();
        
    }
    public void Init(AbilityInfo it)
    {
        item = it;
        if (nameLabel)
        {
            updateUI();
        }
    }

    public void OnEnter()
    {
        string abilityId = item.actionId;
            var level = AbilityManager.Instance.abilityLevel[item.actionId];

        string expString = "At Max Level";
        string details = item.getAbilityDetails;
        if (!AbilityManager.Instance.isAbilityAtMaxLevel(abilityId))
        {
            var exp = AbilityManager.Instance.abilityExp[item.actionId];
            var requiredExp = AbilityManager.Instance.upgradeExp[level];
            expString = string.Format("EXP {0}/{1}", exp, requiredExp);

            details+="\n Next Level: " +item.getNextLevelAbilityDetails;
        }



            AbilityMenu.Instance.detailLabel.text = string.Format("{0} \nLevel {1}   {2}\n{3}",
                item.actionName, level+1, expString, details);
        
    }
    public void OnExit()
    {
        AbilityMenu.Instance.detailLabel.text = "";
    }
    //public void OnClick()
    //{
    //    Inventory.Instance.purchase(item);
    //    ShopManager.Instance.updateShop();
    //}

    // Update is called once per frame
    void Update()
    {
        //button.interactable = Inventory.Instance.canPurchase(item);
    }
}
