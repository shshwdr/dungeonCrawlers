using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    ActionInfo info;

    public TMP_Text battleDialogUI;
    // Start is called before the first frame update
    void Start()
    {
        if (info!=null)
        {
            battleDialogUI.text = info.actionName;
        }
    }

    public void Init(ActionInfo i)
    {
        info = i;
    }

    public void OnClick()
    {
        //check if enough cost
        if (info.getCost > BattleSystem.Instance.skillPoint && !GameManager.Instance.noActionCost)
        {

            HUD.Instance.battleDialogUI.text = string.Format(Dialogs.actionCostNotEnough, info.actionName);
        }
        else if ((info is AbilityInfo) &&  !BattleSystem.Instance.player.canUseMana(((AbilityInfo)info).getMana) && !GameManager.Instance.noManaCost)
        {

            HUD.Instance.battleDialogUI.text = string.Format(Dialogs.actionCostNotEnough, info.actionName);
        }
        else
        {
            if (!GameManager.Instance.noActionCost)
            {

                BattleSystem.Instance.updateSkillPoint(info.getCost);
            }
            if (!GameManager.Instance.noManaCost)
            {
                int manaCost = 0;
                if ((info is AbilityInfo))
                {
                    manaCost = ((AbilityInfo)info).getMana;
                }
                BattleSystem.Instance.player.consumeMana(manaCost);

            }
            HUD.Instance.battleDialogUI.text = info.doActionDesc;
            if (info is ItemInfo)
            {
                var itemInfo = (ItemInfo)info;
                if (itemInfo != null)
                {
                    switch (itemInfo.actionId)
                    {
                        case "hpPotion":
                            BattleSystem.Instance.OnHeal(itemInfo.param);
                            break;
                    }
                    Inventory.Instance.useItem(itemInfo.actionId);
                }
            }
            else if (info is AbilityInfo)
            {
                var abilityInfo = (AbilityInfo)info;
                BattleSystem.Instance.OnAbilityButton(abilityInfo);
            }
            else
            {

                switch (info.actionId)
                {
                    case "absorb":
                        BattleSystem.Instance.OnAbsorbButton();
                        //StartCoroutine( BattleSystem.Instance.OnAbsorb());
                        //BattleSystem.Instance.OnAttackButton();
                        break;
                    case "run":
                        BattleSystem.Instance.OnRun();
                        break;
                    case "continue":
                        BattleSystem.Instance.OnContinue();
                        break;
                    default:
                        Debug.LogError("no action support");
                        break;
                }
            }

        }
    }


    public void OnPointEnter()
    {
        string text = "";
        if (info is AbilityInfo)
        {
            var abilityInfo = (AbilityInfo)info;
            if(info.descriptionType == "attack")
            {

                text = string.Format(info.description, BattleSystem.Instance.getPlayerDamage(abilityInfo));
            }else if (info.descriptionType == "attackEffect")
            {

                text = string.Format(info.description, BattleSystem.Instance.getPlayerDamage(abilityInfo), abilityInfo.getEffectValue);
            }
            else if (info.descriptionType == "effect")
            {

                text = string.Format(info.description, abilityInfo.getEffectValue);
            }
        }
        else if(info.descriptionType == "item")
        {
            var itemInfo = (ItemInfo)info;
            if (itemInfo!=null)
            {

                text = string.Format(info.description, itemInfo.param);
            }
            else
            {
                Debug.LogError("this is not an item!"+info.actionId);
            }
        }
        else
        {
            text = info.description;
        }
        if (info.getCost > 0)
        {
            text += string.Format(Dialogs.actionCost, info.getCost);
        }
        if((info is AbilityInfo) && ((AbilityInfo)info).getMana > 0)
        {

            text += string.Format(Dialogs.manaCost, ((AbilityInfo)info).getMana);
        }
        HUD.Instance.battleDialogUI.text = text;
    }

    public void OnPointExit()
    {

        HUD.Instance.battleDialogUI.text = string.Format(Dialogs.chooseAction, BattleSystem.Instance.skillPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
