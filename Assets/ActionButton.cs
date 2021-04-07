using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    ActionInfo info;

    public TMP_Text battleDialogUI;
    public GameObject dialogUIObj;

    AudioSource audioSource;

    public AudioClip notEnoughSourceClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        if (CSDialogueManager.Instance.isInDialogue)
        {
            return;
        }
        if (!BattleSystem.Instance.isInBattle)
        {
            if (dialogUIObj)
            {
                dialogUIObj.SetActive(false);
            }
            if (info is ItemInfo)
            {
                var itemInfo = (ItemInfo)info;
                if (itemInfo != null)
                {
                    switch (itemInfo.actionId)
                    {
                        case "hpPotion":
                        case "hpPotion1":
                            BattleSystem.Instance.OnHeal(itemInfo.param);
                            break;
                        case "spPotion":
                        case "spPotion1":
                            BattleSystem.Instance.OnRestoreSP(itemInfo.param);
                            break;
                    }
                    Inventory.Instance.useItem(itemInfo.actionId);
                }
            }
            return;
        }
        //check if enough cost
        if (BattleSystem.Instance.isInBattle && info.getCost > BattleSystem.Instance.skillPoint && !GameManager.Instance.noActionCost )
        {

            audioSource.PlayOneShot(notEnoughSourceClip);
            HUD.Instance.battleDialogUI.text = string.Format(Dialogs.actionCostNotEnough, info.actionName);
        }
        else if (BattleSystem.Instance.isInBattle && (info is AbilityInfo) &&  !BattleSystem.Instance.player.canUseMana(((AbilityInfo)info).getMana) && !GameManager.Instance.noManaCost)
        {

            audioSource.PlayOneShot(notEnoughSourceClip);
            HUD.Instance.battleDialogUI.text = string.Format(Dialogs.actionCostNotEnough, info.actionName);
        }
        else
        {
            if (BattleSystem.Instance.isInBattle && !GameManager.Instance.noActionCost)
            {

                BattleSystem.Instance.updateSkillPoint(info.getCost);
            }
            if (BattleSystem.Instance.isInBattle && !GameManager.Instance.noManaCost)
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
                        case "hpPotion1":
                            BattleSystem.Instance.OnHeal(itemInfo.param);
                            break;
                        case "spPotion":
                        case "spPotion1":
                            BattleSystem.Instance.OnRestoreSP(itemInfo.param);
                            break;
                    }
                    Inventory.Instance.useItem(itemInfo.actionId);
                }
            }
            else if (info is AbilityInfo)
            {
                var abilityInfo = (AbilityInfo)info;
                BattleSystem.Instance.OnAbilityButton(abilityInfo);
                var soundstring = "sfx/Spells/" + abilityInfo.actionId;
                var sound = Resources.Load<AudioClip>(soundstring);
                if (sound)
                {

                   BattleSystem.Instance.player.audioSource.PlayOneShot(sound);
                }
                else
                {
                }
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
                    case "item":
                        HUD.Instance.showItemActions();
                        break;
                    case "spell":
                        HUD.Instance.showSkillActions();
                        break;
                    case "back":
                        HUD.Instance.showGeneraActions();
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
        if (dialogUIObj)
        {
            dialogUIObj.SetActive(true);
        }
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
        if (BattleSystem.Instance.isInBattle && info.getCost > 0)
        {
            var cost = info.getCost;
            text += string.Format(Dialogs.actionCost, cost);
        }
        if(BattleSystem.Instance.isInBattle && (info is AbilityInfo) && ((AbilityInfo)info).getMana > 0)
        {

            text += string.Format(Dialogs.manaCost, ((AbilityInfo)info).getMana);
        }
        if (dialogUIObj)
        {
            dialogUIObj.GetComponentInChildren<TMP_Text>().text = text;
        }
        else
        {

            HUD.Instance.battleDialogUI.text = text;
        }
    }

    public void OnPointExit()
    {
        if (dialogUIObj)
        {
            dialogUIObj.SetActive(false);
        }
        HUD.Instance.battleDialogUI.text = string.Format(Dialogs.chooseAction, BattleSystem.Instance.skillPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
